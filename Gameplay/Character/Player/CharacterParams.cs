using System.Collections;
using System.Collections.Generic;
using Assets.HeroEditor.Common.CharacterScripts;
using HeroEditor.Common.Data;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character params", menuName = "Character Params")]
public class CharacterParams : ScriptableObject
{
    [SerializeField]
    private Emotion defaultExpression;
    [SerializeField]
    private Color eyesColor;
    [SerializeField]
    private Sprite hair;
    [SerializeField]
    private Color hairColor;
    [SerializeField]
    private string armorName;
    [SerializeField]
    private Sprite earrings;


    public void SetupParams(Character character)
    {
        var equipments = character.GetComponent<EquipmentControls>();

        if (defaultExpression != null)
        {
            FixDefaultExpression(character);
        }
        character.EyesRenderer.color = eyesColor;

        if (hair != null)
        {
            character.Hair = hair;
            character.HairMask.GetComponent<SpriteRenderer>().color = hairColor;
        }

        character.Earrings = earrings;
        equipments.EquipArmor(armorName);
    }

    private void FixDefaultExpression(Character character)
    {
        character.Expressions[0] = defaultExpression.ToExpression();
    }
}
