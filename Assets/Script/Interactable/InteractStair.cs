using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractStair : MonoBehaviour, IInteractable
{
    public string sceneToLoad;
    public void Interact()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            Debug.Log(sceneToLoad + " 로 이동중...");
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("이동할 씬 이름이 설정되지 않았습니다.");
        }
    }
}
