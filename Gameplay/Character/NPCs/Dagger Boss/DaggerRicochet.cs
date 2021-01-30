using System.Collections;
using System.Collections.Generic;
using Assets.HeroEditor.Common.CharacterScripts;
using UnityEngine;

public class DaggerRicochet : MonoBehaviour
{
    private Projectile projectile;
    private Rigidbody2D rBody;
    private TrailRenderer trail;

    private void Awake()
    {
        projectile = GetComponent<Projectile>();
        rBody = GetComponent<Rigidbody2D>();
        trail = GetComponentInChildren<TrailRenderer>();
    }

    private void OnEnable()
    {
        projectile.hideProjectile = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("DamageToEnemies"))
        {
            trail.gameObject.SetActive(false);

            rBody.velocity = new Vector2(transform.right.x * Mathf.Sign(transform.lossyScale.x), Vector2.up.y) * 12f;
            rBody.angularVelocity = 1800f;
            projectile.hideProjectile = true;
        } else
        {
            projectile.HideBullet();
        }
    }
}
