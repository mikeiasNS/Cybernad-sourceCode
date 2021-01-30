using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimationController : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void Walk(bool toogle)
    {
        animator.SetBool(NPCAnimations.walking , toogle);
    }

    public void Alertness(bool alertness)
    {
        animator.SetBool(NPCAnimations.alertness, alertness);
    }

    public void Attack(bool meleeAttack)
    {
        animator.SetFloat(NPCAnimations.meleeAttack, meleeAttack ? 1 : 0);
        animator.SetTrigger(NPCAnimations.attack);
    }
}
