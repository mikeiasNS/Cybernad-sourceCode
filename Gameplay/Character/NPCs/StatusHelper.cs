using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusHelper
{
    public static float GetInitialForce(ClassType classType)
    {
        if (classType == ClassType.ROGUE) return (float)Attributes.Rogue.force;
        else if (classType == ClassType.WARRIOR) return (float)Attributes.Warrior.force;
        else if (classType == ClassType.SOLDIER) return (float)Attributes.Soldier.force;
        else return (float)Attributes.Sniper.force;
    }

    public static float GetInitialIntelligence(ClassType classType)
    {
        if (classType == ClassType.ROGUE) return (float)Attributes.Rogue.intelligence;
        else if (classType == ClassType.WARRIOR) return (float)Attributes.Warrior.intelligence;
        else if (classType == ClassType.SOLDIER) return (float)Attributes.Soldier.intelligence;
        else return (float)Attributes.Sniper.intelligence;
    }

    public static float GetInitialDexterity(ClassType classType)
    {
        if (classType == ClassType.ROGUE) return (float)Attributes.Rogue.dexterity;
        else if (classType == ClassType.WARRIOR) return (float)Attributes.Warrior.dexterity;
        else if (classType == ClassType.SOLDIER) return (float)Attributes.Soldier.dexterity;
        else return (float)Attributes.Sniper.dexterity;
    }

    public static float GetDefense(float force, float intelligence, float dex)
    {
        return force * 0.6f + intelligence * 0.1f + dex * 0.3f; ;
    }

    public static float GetAttack(float force, float intelligence, float dex)
    {
        return force * 0.5f + intelligence * 0.4f + dex * 0.1f;
    }

    public static class Attributes
    {
        public enum Warrior
        {
            force = 4, intelligence = 1, dexterity = 1
        }
        public enum Rogue
        {
            force = 1, intelligence = 3, dexterity = 2
        }
        public enum Soldier
        {
            force = 2, intelligence = 2, dexterity = 2
        }
        public enum Sniper
        {
            force = 1, intelligence = 4, dexterity = 1
        }
    }
}
