using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Teleporter : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private Image transitionPanel;
    [SerializeField]
    private Transform destination;
    [SerializeField]
    private Transform targetToWalkBefore;
    [SerializeField]
    private UnityEvent inBetween;

    private InputListener inputListener;

    private bool teleporting = false;
    private CharacterMovements playerMov;

    private void Start()
    {
        InitInputListener();
        playerMov = player.GetComponent<CharacterMovements>();
    }

    private void InitInputListener()
    {
        var gameController = GameObject.FindGameObjectWithTag("GameController");
        if(gameController != null)
            inputListener = gameController.GetComponent<InputListener>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == player)
        {
            if (inputListener == null) InitInputListener();
                inputListener.gameObject.SetActive(false);

            if (targetToWalkBefore == null)
                StartCoroutine(FadeIn());
            else
                playerMov.SetTargetToGo(targetToWalkBefore, FinishWalking);
        }
    }

    private bool FinishWalking()
    {
        if (!teleporting)
            StartCoroutine(FadeIn());
        return false;
    }

    IEnumerator FadeIn()
    {
        if (teleporting) yield break;

        teleporting = true;
        transitionPanel.gameObject.SetActive(true);

        transitionPanel.color = new Color(transitionPanel.color.r,
            transitionPanel.color.g, transitionPanel.color.b, 0);

        var iColor = transitionPanel.color;
        while (transitionPanel.color.a < 1)
        {
            transitionPanel.color = new Color(iColor.r, iColor.g, iColor.b, transitionPanel.color.a + 0.1f);
            yield return new WaitForSeconds(0.1f);
        }

        playerMov.RemoveTargetToGo();
        player.transform.position = destination.position;
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        transitionPanel.gameObject.SetActive(true);
        inBetween.Invoke();

        transitionPanel.color = new Color(transitionPanel.color.r,
            transitionPanel.color.g, transitionPanel.color.b, 1);

        var iColor = transitionPanel.color;
        while (transitionPanel.color.a > 0)
        {
            transitionPanel.color = new Color(iColor.r, iColor.g, iColor.b, transitionPanel.color.a - 0.1f);
            yield return new WaitForSeconds(0.1f);
        }

        transitionPanel.gameObject.SetActive(false);
        teleporting = false;
        inputListener.gameObject.SetActive(true);
    }
}
