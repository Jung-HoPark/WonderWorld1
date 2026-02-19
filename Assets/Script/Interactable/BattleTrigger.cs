using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTrigger : MonoBehaviour, IInteractable
{
    public string battleSceneName = "BattleScene";

    public void Interact()
    {
        GameManager.Instance.SavePosition(transform.position, UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        UnityEngine.SceneManagement.SceneManager.LoadScene(battleSceneName);
    }
}
