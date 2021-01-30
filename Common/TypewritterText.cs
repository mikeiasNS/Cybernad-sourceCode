using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TypewritterText : MonoBehaviour
{
    private TextMeshProUGUI textComponentGUI;
    private TextMeshPro textComponentWorld;

    private AudioSource audioSrc;
    private int currentIndex = 0;

    [TextArea][SerializeField]
    private List<string> texts;
    [SerializeField]
    private AudioClip typingSound;
    [SerializeField]
    private bool automatic = true;
    [SerializeField]
    private float timeToNext = 1f;
    [SerializeField]
    private UnityEvent onComplete;

    [SerializeField]
    private SpriteRenderer wrapper;

    private void Awake()
    {
        audioSrc = GetComponent<AudioSource>();
        textComponentGUI = GetComponent<TextMeshProUGUI>();
        textComponentWorld = GetComponent<TextMeshPro>();
    }

    private void Start()
    {
        StartCoroutine(StartTyping());
    }

    public void JumpToEnd()
    {
        StopAllCoroutines();
        TextComponent().text = texts[currentIndex];
    }

    public bool GoToNext()
    {
        if(currentIndex < texts.Count -1)
        {
            currentIndex++;
            StartCoroutine(StartTyping());
            return true;
        }

        return false;
    }

    IEnumerator StartTyping()
    {
        var text = texts[currentIndex];
        TextComponent().text = "<color=#00000000>" + text + "</color>";

        ResizeWrapper();
        PlayTypingSound();

        for (int i=0; i<=text.Length; i++)
        {
            TextComponent().text = text.Substring(0, i) + "<color=#00000000>" + text.Substring(i) + "</color>";
            yield return new WaitForSeconds(0.05f);
        }

        StopTypingSound();
        yield return new WaitForSeconds(timeToNext);
        if(automatic && GoToNext())
        {
            yield break;
        }
        onComplete.Invoke();
    }

    private void ResizeWrapper()
    {
        if(wrapper != null)
        {
            TextComponent().ForceMeshUpdate();

            var textSize = TextComponent().GetPreferredValues(texts[currentIndex]);
            var padding = new Vector2(1, 1);

            Vector2 wrapperSize = textSize + padding;
            wrapper.size = wrapperSize;

            Vector3 textPosition = TextComponent().transform.localPosition;
            wrapper.transform.localPosition = new Vector2(textPosition.x + textSize.x * .5f,
                textPosition.y - textSize.y * .5f);
        }
    }

    private void StopTypingSound()
    {
        if (audioSrc != null && audioSrc.isPlaying)
            audioSrc.Stop();
    }

    private void PlayTypingSound()
    {
        if (audioSrc != null && typingSound != null)
        {
            audioSrc.clip = typingSound;
            audioSrc.Play();
        }
    }

    private TMP_Text TextComponent()
    {
        if (textComponentGUI == null)
            return textComponentWorld;

        return textComponentGUI;
    }
}