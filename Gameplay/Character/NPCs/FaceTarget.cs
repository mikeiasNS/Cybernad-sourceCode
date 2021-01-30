using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceTarget : MonoBehaviour
{
    private float leftScale, rightScale;

    [SerializeField]
    private Transform target;
    [SerializeField][Range(-1, 1)]
    private int direction;

    private void Awake()
    {
        if (direction == (int)EnumDirection.LEFT)
        {
            leftScale = transform.localScale.x;
            rightScale = transform.localScale.x * -1;
        }
        else
        {
            rightScale = transform.localScale.x;
            leftScale = transform.localScale.x * -1;
        }
    }

    private void Update()
    {
        if (!IsFacingTarget())
            Flip();
    }



    private void Flip()
    {
        direction *= -1;

        var scale = transform.localScale;
        scale.x = Mathf.Clamp(direction, leftScale, rightScale);
        transform.localScale = scale;
    }

    bool IsFacingTarget()
    {
        return (target.position.x < transform.position.x && direction < 0) ||
            (target.position.x > transform.position.x && direction > 0);
    }
}
