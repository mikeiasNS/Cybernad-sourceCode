using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAwardSystem : MonoBehaviour
{
    private SpriteRenderer sRenderer;
    private Collider2D coll;

    [SerializeField]
    private EquipmentControls equipmentControls;
    [SerializeField]
    private List<string> gunNames;
    [SerializeField]
    private List<Sprite> gunSprites;
    [SerializeField]
    private GameObject newGunIndicator;
    [SerializeField]
    private GunPanel gunPanel;
    [SerializeField]
    private AudioClip newGunSound;

    private string gunName = "";
    private int currentGunIndex = -1;
    private AudioSource sfxSrc;

    private void Start()
    {
        sfxSrc = GameObject.FindGameObjectWithTag("SFX").GetComponent<AudioSource>();
        sRenderer = GetComponent<SpriteRenderer>();
        coll = GetComponent<Collider2D>();
        currentGunIndex = GetGunIndex();
        if (currentGunIndex >= 0)
        {
            sRenderer.sprite = gunSprites[currentGunIndex];
            gunName = gunNames[currentGunIndex];
        }
        else
            Disable();

        KillCounter.lastPontuation = 0;
    }

    private void Disable()
    {
        coll.enabled = false;
        sRenderer.enabled = false;
    }

    private void Update()
    {
        var index = GetGunIndex();
        if (index > currentGunIndex)
        {
            gunPanel.SetGunParams(gunNames[index]);
            newGunIndicator.SetActive(true);
            equipmentControls.EquipFirearm(gunNames[index]);
            currentGunIndex = index;
            StartCoroutine(HideNewGunIndicator());
            sfxSrc.volume = .8f;
            sfxSrc.PlayOneShot(newGunSound);
        }
    }

    private int GetGunIndex()
    {
        var lastPontuation = KillCounter.lastPontuation;

        if (lastPontuation >= 100)
            return 3;
        else if (lastPontuation >= 50)
            return 2;
        else if (lastPontuation >= 25)
            return 1;
        else if (lastPontuation >= 10)
            return 0;
        else
            return -1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gunName != "")
            equipmentControls.EquipFirearm(gunName);

        Disable();
    }

    IEnumerator HideNewGunIndicator()
    {
        yield return new WaitForSeconds(5f);
        newGunIndicator.SetActive(false);
    }
}
