using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTrigger : MonoBehaviour, IInteractable
{
    public string battleSceneName = "BattleScene";

    public void Interact()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(battleSceneName);
    }
}
