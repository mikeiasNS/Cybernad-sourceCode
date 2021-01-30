using System.Collections;
using System.Collections.Generic;
using Assets.HeroEditor.Common.CharacterScripts;
using UnityEngine;

public class CharacterParamsControls : MonoBehaviour
{
    private SceneLoader sceneLoader;
    private Character character;

    private void Start()
    {
        sceneLoader = FindObjectOfType<SceneLoader>();
        character = FindObjectOfType<Character>();

        if (sceneLoader != null && sceneLoader.nextSceneCharParams != null)
            sceneLoader.nextSceneCharParams.SetupParams(character);
    }
}
