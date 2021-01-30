using System.Collections;
using System.Collections.Generic;
using Assets.HeroEditor.Common.CharacterScripts;
using UnityEngine;

// Kill kill mode
public class EnemyAI : MonoBehaviour, IPoolable
{
    private int direction;
    private NPCMovements movementsController;
    private NPCAnimationController animationsController;
    private CharacterStatus myStatus;
    private AudioSource audioSource;
    private SightController sightController;
    private bool dead = false;
    private Rigidbody2D rBody;

    [SerializeField][Header("General")]
    private ObjectPool myPool;
    [SerializeField]
    private Transform target;
    [SerializeField]
    private float minDistanceToAttack;
    [SerializeField]
    private float maxDistanceToAttack;
    [SerializeField]
    private float maxDistanceToMeleeAttack;
    [SerializeField]
    private float minDistanceToChase;
    [SerializeField]
    private float attackRate;
    [SerializeField]
    private bool alwaysFaceTarget;

    [SerializeField][Header("Shooting")]
    private Transform gunBarrelPosition;
    [SerializeField]
    private Sprite bullet;
    [SerializeField]
    private ObjectPool projectilePool;
    [SerializeField][Range(0, 1)]
    private float accuracy = .95f;
    [SerializeField]
    [Range(0, 5000)]
    private float muzzleVelocity = 18.75f;
    [SerializeField]
    private AudioClip shootSound;

    [SerializeField][Header("Melee")]
    private Transform meleeAttackPosition;
    [SerializeField]
    private float meleeAttackRange;

    public float Accuracy { get { return accuracy; } }
    public float MuzzleVelocity { get { return muzzleVelocity; } }
    public Sprite Bullet { get { return bullet; } }

    private float lastAttack = -1;
    private Camera mainCam;

    private void Awake()
    {
        movementsController = GetComponent<NPCMovements>();
        animationsController = GetComponent<NPCAnimationController>();
        myStatus = GetComponent<CharacterStatus>();
        direction = movementsController.Direction();
        audioSource = GetComponent<AudioSource>();
        sightController = GetComponent<SightController>();
        rBody = GetComponent<Rigidbody2D>();

        mainCam = Camera.main;
    }

    private void Start()
    {
        var bulletPool = GameObject.FindGameObjectWithTag("EnemyBulletPool");
        projectilePool = bulletPool.GetComponent<ObjectPool>();
    }

    void Update()
    {
        if (target != null)
        {
            var targetDamageble = target.GetComponentInParent<Damageble>();
            if(targetDamageble != null && targetDamageble.IsDead())
            {
                target = null;
            }
        }

        int newDirection = movementsController.Direction();
        if (newDirection != 0)
            direction = newDirection;

        animationsController.Alertness(target != null && ReadyToAttack());
        if (target == null)
        {
            movementsController.StopPursuit();
            return;
        }

        var distance = Vector2.Distance(target.position, transform.position);
        if (ReadyToAttack() && CanAttack() && !dead)
        {
            RotateArm();
            lastAttack = Time.time;
            movementsController.Attack(distance <= maxDistanceToMeleeAttack);
        } else if(!ReadyToAttack() && CanChase() && !dead)
        {
            movementsController.TargetToPursuit(target.position);
        } else if(alwaysFaceTarget && !dead)
        {
            movementsController.TargetToFace(target.position);
        }
    }

    private void RotateArm()
    {
        gunBarrelPosition.right = target.position - gunBarrelPosition.position;
    }

    private bool IsFacingTarget()
    {
        return (target.position.x < transform.position.x && direction < 0) ||
            (target.position.x > transform.position.x && direction > 0);
    }

    private bool ReadyToAttack()
    {
        var distance = Vector2.Distance(target.position, transform.position);
        var friendAhead = false;
        var isVisible = false;

        var vpPos = mainCam.WorldToViewportPoint(transform.position);
        if (vpPos.x > 0 && vpPos.x < 1) isVisible = true;

        if(sightController != null)
        {
            foreach(var col in sightController.OnSight())
            {
                if (col != null && col.gameObject.GetInstanceID() != gameObject.GetInstanceID())
                    friendAhead = col.gameObject.GetInstanceID() > gameObject.GetInstanceID();
            }
        }
        var distanceIsOk = distance > minDistanceToAttack && distance < maxDistanceToAttack;

        return IsFacingTarget() && !friendAhead && distanceIsOk && isVisible;
    }

    public void Die()
    {
        if (dead) return;

        dead = true;
        if(target != null)
            target.SendMessage("CountKill", SendMessageOptions.DontRequireReceiver);

        rBody.gravityScale = 0;
        rBody.velocity = Vector2.zero;

        var colliders = GetComponents<Collider2D>();
        foreach(var col in colliders)
            col.enabled = false;

        Invoke("ReturnToPool", 3);
    }

    private bool CanChase() => Vector2.Distance(target.position, transform.position) < minDistanceToChase;

    private bool CanAttack() => Time.time - lastAttack > attackRate || lastAttack == -1;

    public void CreateBullet()
    {
        var iterations = 1;

        audioSource.PlayOneShot(shootSound);
        for (var i = 0; i < iterations; i++)
        {
            var attackDamage = Mathf.RoundToInt(myStatus.attack);

            var bullet = projectilePool.GetObject().GetComponent<Projectile>();
            bullet.GetComponent<Damage>().SetDamage(attackDamage);
            var spread = gunBarrelPosition.up * Random.Range(-1f, 1f) * (1 - Accuracy);

            bullet.transform.position = gunBarrelPosition.position;
            bullet.transform.rotation = gunBarrelPosition.rotation;
            bullet.GetComponent<Rigidbody2D>().velocity = MuzzleVelocity * (gunBarrelPosition.right + spread) * Random.Range(0.85f, 1.15f);
            bullet.GetComponent<SpriteRenderer>().sprite = Bullet;

            bullet.gameObject.layer = LayerMask.NameToLayer("DamageToPlayer");
        }
    }

    public void InflictMeleeAttack()
    {
        var attackDamage = Mathf.RoundToInt(myStatus.attack);

        int targetLayerMask = 1 << target.gameObject.layer;
        var hitted = Physics2D.OverlapCircle(meleeAttackPosition.position,
            meleeAttackRange, targetLayerMask);

        if(hitted)
            hitted.GetComponent<Damageble>().ReceiveDamage(attackDamage);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeAttackRange);
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    private void ReturnToPool()
    {
        if (myPool == null)
            Destroy(gameObject);
        else
            myPool.ReturnObject(gameObject);
    }

    public void SetPool(ObjectPool pool)
    {
        myPool = pool;
    }

    public void OnEntersPool()
    {
        gameObject.SetActive(false);
    }

    public void OnExitPool()
    {
        gameObject.SetActive(true);
        rBody.gravityScale = 1;
        var colliders = GetComponents<Collider2D>();
        foreach (var col in colliders)
            col.enabled = true;

        dead = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy")
        {
            Physics2D.IgnoreCollision(collision.collider, collision.otherCollider);
        }
    }

    public void SetAttackRate(float attackRate)
    {
        this.attackRate = attackRate;
    }

    public void SetMinDistanceToAttack(float distance)
    {
        minDistanceToAttack = distance;
    }

    public void SetMaxDistanceToAttack(float distance)
    {
        maxDistanceToAttack = distance;
    }
}
