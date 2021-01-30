using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Assets.HeroEditor.Common.Data;
using Assets.HeroEditor.Common.EditorScripts;
using System.Linq;
using HeroEditor.Common;

public class GunPanel : MonoBehaviour
{
    [SerializeField]
    private string gunParamsName;

    private FirearmParams gunParams = null;

    public Image image;
    public TextMeshProUGUI title;
    public TextMeshProUGUI dmg;
    public TextMeshProUGUI cadence;
    public TextMeshProUGUI magazine;
    public TextMeshProUGUI reload;

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            gunParams = FirearmCollection.Instance.Firearms.Single(i => i.Name == gunParamsName);
        }
        catch { }

        if (gunParams != null)
            SetFields();
    }

    private void SetFields()
    {
        title.SetText(gunParams.Name);
        image.sprite = GetRifleImage();

        dmg.SetText(CalculateDamage());
        cadence.SetText(CalculateCadence());

        int magazineCapacity = gunParams.MagazineCapacity;
        magazine.SetText(magazineCapacity > 0 ? magazineCapacity.ToString() : "Infinito");
        reload.SetText(gunParams.ReloadAnimation.length.ToString("0.00") + " Seg.");
    }

    private Sprite GetRifleImage()
    {
        List<Sprite> sprites = null;
        try
        {
            sprites = SpriteCollection.Instance.Firearms1H.Single(i => i.Name == gunParams.Name).Sprites;
        }
        catch
        {
            sprites = SpriteCollection.Instance.Firearms2H.Single(i => i.Name == gunParams.Name).Sprites;
        }

        return sprites[4];
    }

    private string CalculateDamage()
    {
        var finalDmg = gunParams.damage;
        if (gunParams.MetaAsDictionary.ContainsKey("Spread"))
            finalDmg *= int.Parse(gunParams.MetaAsDictionary["Spread"]);

        var dmgText = "Baixo";
        var color = Color.green;

        if (finalDmg > 25)
        {
            dmgText = "Moderado";
            color = new Color(1, 0.51f, 0); // orange
        }
        if (finalDmg > 40)
        {
            dmgText = "Alto";
            color = Color.red;
        }

        dmg.color = color;
        return dmgText;
    }

    private string CalculateCadence()
    {
        var cadenceStr = "Baixa";
        var cadence = gunParams.FireRateInMinute;
        var color = Color.green;
        if (cadence >= 300)
        {
            cadenceStr = "Moderada";
            color = new Color(1, 0.51f, 0); // orange
        }
        if (cadence >= 500)
        {
            cadenceStr = "Alta";
            color = Color.red;
        }

        this.cadence.color = color;
        return cadenceStr;
    }

    public void SetGunParams(FirearmParams gunParams)
    {
        this.gunParams = gunParams;
        SetFields();
    }

    public void SetGunParams(string name)
    {
        try
        {
            SetGunParams(FirearmCollection.Instance.Firearms.Single(i => i.Name == name));
        }
        catch { }
    }
}
 