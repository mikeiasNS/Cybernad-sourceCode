using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.HeroEditor.Common.CharacterScripts;
using Assets.HeroEditor.Common.EditorScripts;
using HeroEditor.Common;
using UnityEngine;

public class EquipmentControls : MonoBehaviour
{
    private string currentGun = "LightHandBlaster";
    private string currentArmor = "MARK-H4B [Paint]";

    private Character character;

    private void Awake()
    {
        character = GetComponent<Character>();
    }

    private void Start()
    {
        SetupEquipment();
    }

    private void SetupEquipment()
    {
        EquipArmor(currentArmor);
        EquipFirearm(currentGun);
    }

    public void EquipArmor(string armorName)
    {
        SpriteGroupEntry spriteGroup = SpriteCollection.Instance.Armor.SingleOrDefault(i => i.Name == armorName);

        if (spriteGroup == null) return;
        List<Sprite> armorSprites = spriteGroup.Sprites;

        if (armorSprites.Count > 0)
        {
            currentArmor = name;
            character.EquipArmor(armorSprites);
        }
    }

    //equipFireArm()
    //{

    //}

    public void EquipFirearm(string sName)
    {
        var firearmParams = FirearmCollection.Instance.Firearms.Single(i => i.Name == sName);

        List<Sprite> sprites = null;
        var twoHanded = false;
        try
        {
            sprites = SpriteCollection.Instance.Firearms1H.Single(i => i.Name == sName).Sprites;

        } catch
        {
            sprites = SpriteCollection.Instance.Firearms2H.Single(i => i.Name == sName).Sprites;
            twoHanded = true;
        }

        if(sprites.Count > 0)
        {
            currentGun = sName;
            character.EquipFirearm(sprites, firearmParams, twoHanded);
        }
    }

    //public void EquipFirearm2H(string sName)
    //{
    //    var sprites = SpriteCollection.Instance.Firearms2H.Single(i => i.Name == sName).Sprites;
    //    var firearmParams = FirearmCollection.Instance.Firearms.Single(i => i.Name == sName);

    //    if (sprites.Count > 0)
    //    {
    //        currentGun = sName;
    //        character.EquipFirearm(sprites, firearmParams, true);
    //    }
    //}
}
