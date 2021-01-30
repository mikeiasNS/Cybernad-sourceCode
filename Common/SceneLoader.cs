using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public CharacterParams nextSceneCharParams { get; private set; }

    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void SetNextSceneCharParams(CharacterParams characterParams)
    {
        nextSceneCharParams = characterParams;
    }
}
