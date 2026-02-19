using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    [Header("UI Groups")]
    public CanvasGroup textCanvasGroup;
    public CanvasGroup buttonCanvasGroup;

    [Header("Timing Settings")]
    public float animationDuration = 1.433f;
    public float fadeSpeed = 0.8f;


    // Start is called before the first frame update
    void Start()
    {
        textCanvasGroup.alpha = 0f;
        buttonCanvasGroup.alpha = 0f;
        buttonCanvasGroup.interactable = false;
        buttonCanvasGroup.blocksRaycasts = false;
        
        StartCoroutine(PlayGameOverSequence());
    }

    IEnumerator PlayGameOverSequence()
    {
        yield return new WaitForSeconds(animationDuration);

        while (textCanvasGroup.alpha < 1f)
        {
            textCanvasGroup.alpha += Time.deltaTime * fadeSpeed;
            yield return null;
        }

        yield return new WaitForSeconds(0.3f);

        while (buttonCanvasGroup.alpha < 1f)
        {
            buttonCanvasGroup.alpha += Time.deltaTime * fadeSpeed;
            yield return null;
        }
        buttonCanvasGroup.interactable = true;
        buttonCanvasGroup.blocksRaycasts = true;
    }

    public void OnRestartClick() => GameManager.Instance.RestartGame();
    public void OnQuitClick()
    {
        Debug.Log("게임 종료");
        Application.Quit();
    }
}
