using System;
using System.Collections;
using System.Collections.Generic;
using Assets.HeroEditor.Common.CharacterScripts;
using UnityEngine;

public class ArmRotation : MonoBehaviour
{
    [SerializeField]
    private Transform armR;
    [SerializeField]
    private Transform targetTransform;

    private Character character;
    private GunControls gunControls;
    private Vector2 target;

    public float angleMin = -40;
    public float angleMax = 40;

    private void Awake()
    {
        character = GetComponent<Character>();
        gunControls = GetComponent<GunControls>();
    }

    private void LateUpdate()
    {
        if (gunControls != null && !gunControls.Aiming) return;
        if (gunControls != null)
            target = gunControls.targetPosition;
        else target = targetTransform.position;

        RotateArm(armR, character.Firearm.FireTransform, target, angleMin, angleMax);
    }

    public void RotateTo(Vector2 pos)
    {
        target = pos;
        RotateArm(armR, character.Firearm.FireTransform, target, angleMin, angleMax);
    }

    /// <summary>
    /// Selected arm to position (world space) rotation, with limits.
    /// </summary>
    public void RotateArm(Transform arm, Transform weapon, Vector2 target, float angleMin, float angleMax) // TODO: Very hard to understand logic
    {
        target = arm.transform.InverseTransformPoint(target);

        var angleToTarget = Vector2.SignedAngle(Vector2.right, target);
        var angleToFirearm = Vector2.SignedAngle(weapon.right, arm.transform.right) * Math.Sign(weapon.lossyScale.x);
        var angleFix = Mathf.Asin(weapon.InverseTransformPoint(arm.transform.position).y / target.magnitude) * Mathf.Rad2Deg;
        var angle = angleToTarget + angleToFirearm + angleFix;

        angleMin += angleToFirearm;
        angleMax += angleToFirearm;

        var z = arm.transform.localEulerAngles.z;

        if (z > 180) z -= 360;

        if (z + angle > angleMax)
        {
            angle = angleMax;
        }
        else if (z + angle < angleMin)
        {
            angle = angleMin;
        }
        else
        {
            angle += z;
        }

        arm.transform.localEulerAngles = new Vector3(0, 0, angle);
    }
}
