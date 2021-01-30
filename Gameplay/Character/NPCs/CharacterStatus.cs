using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatus : MonoBehaviour
{
    private float force;
    private float intelligence;
    private float dexterity;
    private float defense;
    public float attack { get; private set; }

    [SerializeField][Range(1, 100)]
    private int level = 1;
    [SerializeField]
    private ClassType classType;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        force = StatusHelper.GetInitialForce(classType) * level;
        intelligence = StatusHelper.GetInitialIntelligence(classType) * level;
        dexterity = StatusHelper.GetInitialDexterity(classType) * level;

        defense = StatusHelper.GetDefense(force, intelligence, dexterity);
        attack = StatusHelper.GetAttack(force, intelligence, dexterity);
    }

    public void SetLevel(int level)
    {
        this.level = level;
        Initialize();
    }

    public float ApplyDefense(float originDamage)
    {
        return originDamage * (100 / (100 + defense));
    }
}
