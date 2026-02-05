using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroSceneManager : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public string nextSceneName = "Stage1";
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("IntroPlayed", 0) == 1)
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {

        }
    }
    public void OnIntroComplete()
    {
        PlayerPrefs.SetInt("IntroPlayed", 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene(nextSceneName);
    }
}
