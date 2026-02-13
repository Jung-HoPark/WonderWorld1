using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransferEvent : MonoBehaviour, IInteractable
{
    public string targetSceneName; // 이동할 씬 이름
    public bool isGameClear = false; // 게임 클리어용인지 체크

    public void Interact()
    {
        if (isGameClear)
        {
            Debug.Log("게임 클리어!");
            GameManager.Instance.LoadNextStage("GameClear");
        }
        else
        {
            Debug.Log($"{targetSceneName}으로 이동합니다.");
            GameManager.Instance.LoadNextStage(targetSceneName);
        }
    }
}
