using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicController : MonoBehaviour
{
    [SerializeField]
    private List<AudioClip> combatSongs;

    AudioSource musicSrc;

    private void Start()
    {
        musicSrc = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();

        SetSceneAudio(SceneManager.GetActiveScene().name);
        SceneManager.activeSceneChanged += ChangedActiveScene;
    }

    public void GameOver()
    {
        musicSrc.Stop();
    }

    public void Restart()
    {
        musicSrc.Play();
    }

    private void ChangedActiveScene(Scene current, Scene next)
    {
        string nextName = next.name;
        SetSceneAudio(nextName);
    }

    private void SetSceneAudio(string nextName)
    {
        if (nextName == "Head quarters")
        {
            StartCoroutine(MusicFadeOut(.1f));
        }
        else if (nextName == "Street" && combatSongs.Count > 0)
        {
            musicSrc.Stop();
            musicSrc.volume = .3f;
            musicSrc.clip = combatSongs[0];
            musicSrc.Play();
        }
    }

    IEnumerator MusicFadeOut(float until = 0)
    {
        while (musicSrc.volume > until)
        {
            musicSrc.volume -= 0.05f;
            yield return new WaitForSecondsRealtime(.1f);
        }
    }
}
