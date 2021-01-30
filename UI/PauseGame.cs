using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    [SerializeField]
    GameObject pausePanel;

    private bool isPaused = false;

    // Update is called once per frame
    void Update()
    {
        bool pauseButtonPressed = Input.GetButtonDown("Pause");
        if (pauseButtonPressed && isPaused)
        {
            Time.timeScale = 1;
            isPaused = false;
            pausePanel.SetActive(isPaused);
        }
        else if (pauseButtonPressed)
        {
            Time.timeScale = 0;
            isPaused = true;
            pausePanel.SetActive(isPaused);
        }
        AudioListener.pause = isPaused;
    }
}
