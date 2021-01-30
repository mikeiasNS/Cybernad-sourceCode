using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.HeroEditor.Common.CharacterScripts;

public class TrooperAnimationListener : MonoBehaviour
{
    private EnemyAI ai;

    private void Awake()
    {
        ai = GetComponentInParent<EnemyAI>();
    }

    public void CallAIMethod(string method)
    {
        ai.SendMessage(method);
    }
}