using System;
using System.Collections;
using System.Collections.Generic;
using Assets.HeroEditor.Common.CharacterScripts;
using HeroEditor.Common.Enums;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CharacterMovements))]
public class GunControls : MonoBehaviour
{
    [SerializeField]
    private float timeToStopAim = 1.5f;
    [SerializeField]
    private TouchHandler touchHandler;

    private Character character;
    private CharacterMovements charMovements;
    private ArmRotation armRotation;

    private bool locked;
    public bool Aiming { get; private set; }
    public Vector2 targetPosition { get; private set; }
    private float lastShoot;

    public BooleanEvent onToggleAim = new BooleanEvent();

    private void Awake()
    {
        character = GetComponent<Character>();
        charMovements = GetComponent<CharacterMovements>();
        armRotation = GetComponent<ArmRotation>();
    }

    private void Update()
    {
        locked = !character.Animator.GetBool("Ready") || character.Animator.GetInteger("Dead") > 0;
        if (locked) return;

        if (touchHandler.Touching || touchHandler.TouchStarted)
        {
            if(!Aiming)onToggleAim.Invoke(true);
            Aiming = true;
            lastShoot = Time.timeSinceLevelLoad;
            targetPosition = Camera.main.ScreenToWorldPoint(touchHandler.TouchPosition);
            charMovements.LookAt(targetPosition);
            armRotation.RotateTo(targetPosition);
        }

        if(touchHandler.TouchEnded) StartCoroutine(ResetAim());

        switch (character.WeaponType)
        {
            case WeaponType.Firearms1H:
            case WeaponType.Firearms2H:
                character.Firearm.Fire.FireButtonDown = touchHandler.TouchStarted;
                character.Firearm.Fire.FireButtonPressed = touchHandler.Touching;
                character.Firearm.Fire.FireButtonUp = touchHandler.TouchEnded;
                character.Firearm.Reload.ReloadButtonDown = Input.GetKeyDown("r");
                break;
            case WeaponType.Supplies:
                if (touchHandler.TouchStarted)
                {
                    character.Animator.Play(Time.frameCount % 2 == 0 ? "UseSupply" : "ThrowSupply", 0); // Play animation randomly
                }
                break;
        }
    }

    IEnumerator ResetAim()
    {
        var previousShoot = lastShoot;
        yield return new WaitForSeconds(timeToStopAim);
        Aiming &= Math.Abs(lastShoot - previousShoot) > 0;
        if(!Aiming) onToggleAim.Invoke(false);
    }
}
