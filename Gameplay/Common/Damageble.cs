using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Damageble : MonoBehaviour
{
    private int currentHitPoints;

    [SerializeField]
    private int maxHitPoints = 100;
    [SerializeField]
    private UnityEvent gotHit;
    [SerializeField]
    private UnityEvent gotDestroyed;
    [SerializeField]
    private Image lifeBar;

    private CharacterStatus characterStatus;

    // Start is called before the first frame update
    void Start()
    {
        currentHitPoints = maxHitPoints;
        characterStatus = GetComponent<CharacterStatus>();
    }

    private void Update()
    {
        if (lifeBar != null)
        {
            float v = currentHitPoints / (float)maxHitPoints;
            lifeBar.fillAmount = v;

            if (v > .8f) lifeBar.color = Color.green;
            else if (v > .6) lifeBar.color = new Color(1, 0.87f, 0); // yellow
            else if (v > .4) lifeBar.color = new Color(1, 0.51f, 0); // orange
            else if (v > 0) lifeBar.color = Color.red;
        }
    }

    public void ReceiveDamage(int damage)
    {
        var finalDamage = characterStatus != null ?
            characterStatus.ApplyDefense(damage) : damage;

        currentHitPoints -= Mathf.RoundToInt(finalDamage);
        if (currentHitPoints <= 0) GetDestroyed();

        gotHit.Invoke();
    }

    public bool IsDead()
    {
        return currentHitPoints <= 0;
    }

    private void GetDestroyed()
    {
        gotDestroyed.Invoke();
    }

    public void Respawn()
    {
        currentHitPoints = maxHitPoints;
    }
}
