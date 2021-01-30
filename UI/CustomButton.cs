using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomButton : MonoBehaviour
{
    [SerializeField]
    private AudioClip click;

    private AudioSource sfxSrc = null;
    private Button buttonComponent;

    private void Start()
    {
        buttonComponent = GetComponent<Button>();
        sfxSrc = GameObject.FindGameObjectWithTag("SFX").GetComponent<AudioSource>();

        buttonComponent.onClick.AddListener(PlayButtonClickSound);
    }

    private void PlayButtonClickSound()
    {
        if(sfxSrc != null && click != null)
            sfxSrc.PlayOneShot(click);
    }
}
