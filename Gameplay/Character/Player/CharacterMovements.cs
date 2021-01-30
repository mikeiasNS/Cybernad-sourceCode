using System;
using System.Collections;
using System.Collections.Generic;
using Assets.HeroEditor.Common.CharacterScripts;
using UnityEngine;

public class CharacterMovements : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 5;
    [SerializeField]
    private float jumpForce = 5;
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private LayerMask floorLayer;

    private Character character;
    private GunControls gunControl;
    private Rigidbody2D rBody;
    private int direction;
    private MovementAnimationName movementAnimation = MovementAnimationName.Run;
    private EmotionHandler emotionHandler;
    private Damageble damageble;

    private Vector2 startPosition;
    private Transform targetToGo = null;
    private Func<bool> gotToTarget;
    private bool targetReached;

    private void Awake()
    {
        character = GetComponent<Character>();
        rBody = GetComponent<Rigidbody2D>();
        gunControl = GetComponent<GunControls>();
        emotionHandler = GetComponent<EmotionHandler>();
        damageble = GetComponent<Damageble>();

        gunControl.onToggleAim.AddListener(OnToggleAim);
    }

    private void Update()
    {
        if (IsDead()) return;

        bool moving = direction > 0f || direction < 0f;

        if (gunControl.Aiming)
        {
            SetMovementAnimation(MovementAnimationName.Walk);
        }
        else
        {
            SetMovementAnimation(MovementAnimationName.Run);
            Flip();
        }

        character.Animator.SetBool(movementAnimation.ToString(), moving && IsGrounded() || (targetToGo != null && targetReached == false));
        character.Animator.SetBool(PlayerAnimations.jump, !IsGrounded() && targetToGo == null);
    }

    void OnToggleAim(bool aiming)
    {
        if (aiming && !IsDead()) emotionHandler.SetExpression("Shooting");
        else if(!IsDead()) emotionHandler.SetExpression("Default");

    }

    private void SetMovementAnimation(MovementAnimationName animationName)
    {
        if(movementAnimation != animationName)
        {
            character.Animator.SetBool(movementAnimation.ToString(), false);
            movementAnimation = animationName;
        }
    }

    private void FixedUpdate()
    {
        float speed = movementAnimation == MovementAnimationName.Walk && IsGrounded() ?
            movementSpeed * 0.5f : movementSpeed;

        if(targetToGo != null)
            MoveToTarget();
        else
        {
            rBody.velocity = new Vector2(speed * direction, rBody.velocity.y); 
        }
    }

    private void MoveToTarget()
    {
        var speedX = movementSpeed * 0.5f;
        var speedY = speedX;

        float myX = transform.position.x;
        float targetX = targetToGo.position.x;

        if (targetX < myX) speedX *= -1;
        if ((myX > startPosition.x && myX > targetX) || (myX < startPosition.x && myX < targetX))
            speedX = 0;

        float myY = transform.position.y;
        float targetY = targetToGo.position.y;

        if (targetY < myY) speedY *= -1;
        if ((myY > startPosition.y && myY > targetY) || (myY < startPosition.y && myY < targetY))
            speedY = 0;

        if (speedX == 0 && speedY == 0)
        {
            targetReached = true;
            if (gotToTarget())
                RemoveTargetToGo();
        }
        rBody.velocity = new Vector2(speedX, speedY);
    }

    public void SetTargetToGo(Transform target, Func<bool> onFinish)
    {
        gotToTarget = onFinish;
        StartCoroutine(StartAutoFollowTarget(target));
    }

    public void RemoveTargetToGo()
    {
        targetToGo = null;
        rBody.gravityScale = 2;
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.1f, floorLayer);
    }

    private void Flip()
    {
        if (direction != 0)
        {
            var scale = transform.localScale;
            scale.x = direction;
            transform.localScale = scale;
        }
    }

    public void LookAt(Vector2 position)
    {
        var scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x);

        if (position.x < transform.position.x) scale.x *= -1;

        transform.localScale = scale;
    }

    public void Jump()
    {
        if (IsGrounded())
            rBody.velocity = new Vector2(rBody.velocity.x, jumpForce);
    }

    public void Move(int direction)
    {
        this.direction = Mathf.Clamp(direction, -1, 1);
    }

    public void Hit()
    {
        if (!IsDead())
            StartCoroutine(GetHit());
    }

    public void Die()
    {
        direction = 0;
        character.Animator.SetBool(movementAnimation.ToString(), false);
        character.Animator.SetBool("Jump", false);
        character.Animator.SetBool("DieBack", true);
        character.Animator.SetInteger("Dead", 1);
        emotionHandler.SetExpression("Dead");

        var controllers = GameObject.FindGameObjectWithTag("GameController");

        if(controllers != null) 
            controllers.SetActive(false);
    }

    IEnumerator StartAutoFollowTarget(Transform target)
    {
        while (!IsGrounded())
            yield return new WaitForSeconds(0.1f);

        targetToGo = target;
        rBody.gravityScale = 0;
        startPosition = transform.position;
        targetReached = false;
    }

    IEnumerator GetHit()
    {
        emotionHandler.SetExpression("Hit");
        character.Animator.SetTrigger(PlayerAnimations.hit);
        yield return new WaitForSeconds(0.8f);
        if (!IsDead())
        {
            emotionHandler.SetExpression(gunControl.Aiming ? "Shooting" : "Default");
        }
    }

    public bool IsDead()
    {
        return damageble.IsDead();
    }

    public int Direction()
    {
        return direction;
    }
}
