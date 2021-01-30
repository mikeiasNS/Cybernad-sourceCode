using System.Collections;
using System.Collections.Generic;
using Assets.HeroEditor.Common.CharacterScripts;
using UnityEngine;

public class DaggerManAI : MonoBehaviour
{
    private CharacterStatus myStatus;
    private NPCMovements movementsController;
    private List<int> defendedBullets = new List<int>();

    [SerializeField]
    private Transform target;
    [SerializeField]
    private Transform daggerPosition;
    [SerializeField]
    private ObjectPool projectilePool;
    [SerializeField]
    private float Accuracy;
    [SerializeField]
    private float MuzzleVelocity;
    [SerializeField]
    private Sprite daggerSprite;
    [SerializeField]
    private float defensiveRange;


    private void Start()
    {
        myStatus = GetComponent<CharacterStatus>();
        movementsController = GetComponent<NPCMovements>();
    }

    private void Update()
    {
        if (ShouldDefend()) movementsController.Attack(false);
    }

    public void AttackTarget()
    {
        AimAt(target.position);
        movementsController.Attack(false);
    }

    bool ShouldDefend()
    {
        int layerMask = 1 << LayerMask.NameToLayer("DamageToEnemies");

        float centerX = transform.position.x + defensiveRange * 0.5f * Mathf.Sign(transform.lossyScale.x);
        var center = new Vector2(centerX, transform.position.y);
        var bullet = Physics2D.OverlapBox(center, new Vector2(defensiveRange, 4), 0, layerMask);

        if(bullet)
        {
            var bulletId = bullet.gameObject.GetInstanceID();
            if (defendedBullets.Contains(bulletId))
                return false;
            defendedBullets.Add(bulletId);
            StartCoroutine(RemoveDefendedBullet(bulletId, 0.5f));
            AimAt(bullet.transform.position); 
        }

        return bullet;
    }

    private void  AimAt(Vector2 position)
    {
        position = daggerPosition.transform.InverseTransformPoint(position);

        var angleToTarget = Vector2.SignedAngle(Vector2.right, position);
        var angleFix = Mathf.Asin(daggerPosition.InverseTransformPoint(daggerPosition.transform.position).y / position.magnitude) * Mathf.Rad2Deg;
        var angle = angleToTarget + angleFix;

        var angleMax = 40;
        var angleMin = -40;

        var z = daggerPosition.transform.localEulerAngles.z;

        if (z > 180) z -= 360;

        if (z + angle > angleMax)
            angle = angleMax;
        else if (z + angle < angleMin)
            angle = angleMin;
        else
            angle += z;

        daggerPosition.transform.localEulerAngles = new Vector3(0, 0, angle);
    }

    public void CreateDagger()
    {
        var attackDamage = Mathf.RoundToInt(myStatus.attack);

        var dagger = projectilePool.GetObject().GetComponent<Projectile>();
        dagger.GetComponent<Damage>().SetDamage(attackDamage);

        var spread = daggerPosition.up * Random.Range(-1f, 1f) * (1 - Accuracy);

        dagger.transform.position = daggerPosition.position;
        dagger.transform.rotation = daggerPosition.rotation;
        dagger.transform.localScale = new Vector2(dagger.transform.localScale.x * -Mathf.Sign(transform.lossyScale.x), dagger.transform.localScale.y);
        dagger.GetComponent<Rigidbody2D>().angularVelocity = 0f;
        dagger.GetComponent<Rigidbody2D>().velocity = MuzzleVelocity * (daggerPosition.right + spread)
            * Mathf.Sign(transform.lossyScale.x) * Random.Range(0.85f, 1.15f);
        dagger.GetComponent<SpriteRenderer>().sprite = daggerSprite;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        float centerX = transform.position.x + defensiveRange * 0.5f * Mathf.Sign(transform.lossyScale.x);
        var center = new Vector2(centerX, transform.position.y);
        Gizmos.DrawWireCube(center, new Vector3(defensiveRange, 4));
    }

    private IEnumerator RemoveDefendedBullet(int bulletId, float delayedTime)
    {
        yield return new WaitForSecondsRealtime(delayedTime);
        defendedBullets.Remove(bulletId);
    }
}
