using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputListener : MonoBehaviour
{
    [SerializeField]
    private CharacterMovements characterMovements;

    private bool jump = false;

    private void Update()
    {
        if (!jump)
            jump = Input.GetButtonDown("Jump");
    }

    private void FixedUpdate()
    {
        if (characterMovements.IsDead()) return;

        var horizontal = Input.GetAxisRaw("Horizontal");
        characterMovements.Move((int)horizontal);

        if(jump)
        {
            jump = false;
            characterMovements.Jump();
        }
    }

    private void OnDisable()
    {
        characterMovements.Move(0);
    }
}
