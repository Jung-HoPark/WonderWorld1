using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroSceneManager : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public string nextSceneName = "Stage1";

    void Awake()
    {
        // 테스트용: 게임을 켤 때마다 인트로를 다시 보고 싶다면 주석 해제
        // IntroDialogue.hasPlayedIntro = false; 

        // 영구 저장된 기록을 지우고 싶을 때 (PlayerPrefs)
        // PlayerPrefs.DeleteKey("IntroPlayed"); 
    }
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
