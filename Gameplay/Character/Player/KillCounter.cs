using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KillCounter : MonoBehaviour
{
    public static int lastPontuation = 0;
    public static int bestPontuation = 0;

    [SerializeField]
    TextMeshProUGUI killsText;
    [SerializeField]
    TextMeshProUGUI gameOverKillsText;
    [SerializeField]
    TextMeshProUGUI gameOverBestScore;

    private int kills;

    void CountKill()
    {
        kills++;
        lastPontuation++;

        if (kills > bestPontuation)
            bestPontuation = kills;

        killsText.SetText("Mortes: " + kills);
        gameOverKillsText.SetText("Mortes: " + kills);
        gameOverBestScore.SetText("Melhor: " + bestPontuation);
    }
}
