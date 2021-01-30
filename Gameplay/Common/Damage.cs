using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField]
    private int damage;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var damageble = collision.gameObject.GetComponent<Damageble>();

        if(damageble != null)
            damageble.ReceiveDamage(damage);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var damageble = collision.gameObject.GetComponent<Damageble>();

        if (damageble != null)
            damageble.ReceiveDamage(damage);
    }

    public void SetDamage(int damage)
    {
        this.damage = damage;
    }
}
