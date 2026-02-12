using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [Header("Core Data")]
    public PlayerResourceManager playerRes;
    public List<CharacterData> partyMembers;

    [Header("Scene Settings")]
    public string introSceneName = "StartOnlyOnce";
    public string stage1Name = "Stage1";
    public string stage2Name = "Stage2";
    public string gameOverSceneName = "GameOver";

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
    void Update()
    {
        CheckGameOver();
    }
    void CheckGameOver()
    {
        if (playerRes != null && playerRes.playerHeart <= 0)
        {
            TriggerGameOver();
        }
    }
    public void TriggerGameOver()
    {
        Debug.Log("플레이어 사망! 게임오버 씬으로 이동합니다.");
        // 사망 시 데이터 초기화 후 씬 이동
        SceneManager.LoadScene(gameOverSceneName);
    }
    public void LoadNextStage(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
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
