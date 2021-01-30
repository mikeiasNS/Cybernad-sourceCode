using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class ExpressionMakerManager : MonoBehaviour
{
    public SpriteRenderer eyeBrowsRenderer;
    public SpriteRenderer eyesRenderer;
    public SpriteRenderer mouthRenderer;
    public InputField nameInput;

    public void SetEyes(Image eyes)
    {
        eyesRenderer.sprite = eyes.sprite;
    }

    public void SetEyebrows(Image image)
    {
        eyeBrowsRenderer.sprite = image.sprite;
    }

    public void SetMouth(Image image)
    {
        mouthRenderer.sprite = image.sprite;
    }

    public void Save()
    {
#if (UNITY_EDITOR)
        var emotion = new Emotion(nameInput.text, eyeBrowsRenderer.sprite,
            eyesRenderer.sprite, mouthRenderer.sprite);

        AssetDatabase.CreateAsset(emotion, "Assets/Scriptable objects/Emotions/" + nameInput.text + ".asset");
#endif
    }
}
