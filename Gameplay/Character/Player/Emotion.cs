using System;
using HeroEditor.Common.Data;
using UnityEngine;

[CreateAssetMenu(fileName = "New Emotion", menuName = "Emotion")]
public class Emotion : ScriptableObject
{
    [SerializeField]
    private string name;
    [SerializeField]
    private Sprite eyebrows;
    [SerializeField]
    private Sprite eyes;
    [SerializeField]
    private Sprite mouth;

    public Emotion(string name, Sprite eyebrows, Sprite eyes, Sprite mouth)
    {
        this.name = name;
        this.eyebrows = eyebrows;
        this.eyes = eyes;
        this.mouth = mouth;
    }

    public Expression ToExpression()
    {
        var expression = new Expression
        {
            Name = name,
            Eyebrows = eyebrows,
            Eyes = eyes,
            Mouth = mouth
        };

        return expression;
    }
}
