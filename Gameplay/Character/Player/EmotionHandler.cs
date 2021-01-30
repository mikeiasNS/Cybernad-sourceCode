using System.Collections;
using System.Collections.Generic;
using Assets.HeroEditor.Common.CharacterScripts;
using UnityEngine;

public class EmotionHandler : MonoBehaviour
{
    [SerializeField]
	private List<Emotion> emotions;
    [SerializeField]
    private string currentEmotion = "Default";

    private Character character;

    private void Awake()
    {
        character = GetComponent<Character>();
        FetchCharacterExpressions();
    }

    public void SetExpression(string name)
    {
        currentEmotion = name;
        character.SetExpression(currentEmotion);
    }

    private void FetchCharacterExpressions()
    {
        foreach (Emotion emotion in emotions)
        {
            character.Expressions.Add(emotion.ToExpression());
        }
    }
}
