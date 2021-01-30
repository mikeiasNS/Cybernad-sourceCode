using UnityEngine;

public class NPCMovements : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed;
    [SerializeField][Range(-1, 1)]
    private int direction;

    public bool canMove = false;

    private Vector2 targetToPursuit;
    private bool pursuiting = false;
    private Rigidbody2D rBody;
    private float leftScale, rightScale;
    private NPCAnimationController npcAnimationController;

    private void Awake()
    {
        rBody = GetComponent<Rigidbody2D>();
        npcAnimationController = GetComponent<NPCAnimationController>();

        if (direction == (int)EnumDirection.LEFT)
        {
            leftScale = transform.localScale.x;
            rightScale = transform.localScale.x * -1;
        } else
        {
            rightScale = transform.localScale.x;
            leftScale = transform.localScale.x * -1;
        }
    }

    private void Start()
    {
        targetToPursuit = transform.position;
    }

    void Update()
    {
        MoveToTarget();
        Flip(direction);
    }

    private void FixedUpdate()
    {
        if(canMove)
            rBody.velocity = new Vector2(movementSpeed * direction, rBody.velocity.y);
        else
            rBody.velocity = new Vector2(0, rBody.velocity.y);
    }

    private void MoveToTarget()
    {
        if (pursuiting && targetToPursuit != null)
            direction = DirectionForTarget(targetToPursuit);
        else
            direction = 0;

        npcAnimationController.Walk(direction != 0);
    }

    private int DirectionForTarget(Vector2 target)
    {
        var currentX = transform.position.x;
        if (currentX < target.x)
            return (int)EnumDirection.RIGHT;
        else if (currentX > target.x)
            return (int)EnumDirection.LEFT;
        else
            return 0;
    }

    private void Flip(int dir)
    {
        if (dir != 0)
        {
            var scale = transform.localScale;
            scale.x = Mathf.Clamp(dir, leftScale, rightScale);
            transform.localScale = scale;
        }
    }

    public void TargetToPursuit(Vector2 targetPosition)
    {
        pursuiting = true;
        targetToPursuit = targetPosition;
    }

    public void StopPursuit()
    {
        pursuiting = false;
    }

    public int TargetToFace(Vector2 targetPosition)
    {
        direction = DirectionForTarget(targetPosition);
        Flip(direction);

        return direction;
    }

    public void Attack(bool meleeAttack)
    {
        pursuiting = false;
        npcAnimationController.Attack(meleeAttack);
    }

    public int Direction()
    {
        return direction;
    }
}
