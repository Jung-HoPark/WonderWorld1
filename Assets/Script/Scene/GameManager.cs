using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [Header("Core Data")]
    public PlayerResourceManager playerRes;

    [Header("Scene Settings")]
    public string introSceneName = "StartOnlyOnce";
    public string stage1Name = "Stage1";
    public string stage2Name = "Stage2";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        InitializeGame();
    }
    public void InitializeGame()
    {
        if (playerRes != null)
        {
            playerRes.ResetData();
            Debug.Log("플레이어 데이터가 초기화되었습니다.");

            // 2. [선택] 만약 인트로도 다시 보고 싶다면 아래 주석을 해제
            PlayerPrefs.DeleteKey("IntroPlayed");
        }
    }
    public void RestartGame()
    {
        InitializeGame();
        SceneManager.LoadScene(introSceneName);
    }
    public void HardRestart()
    {
        PlayerPrefs.DeleteAll(); // 모든 기록 삭제 (인트로 포함)
        InitializeGame();
        SceneManager.LoadScene("IntroScene");
    }
}
