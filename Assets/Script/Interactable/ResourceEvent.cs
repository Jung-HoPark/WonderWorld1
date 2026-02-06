using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceEvent : MonoBehaviour, IInteractable
{
    public PlayerResourceManager playerRes;
    public DialogueManager dialogueManager;
    public PlayerController player;

    [Header("자원 변경량")]
    public int hpChange;
    public float mpChange;
    public int expChange;

    [Header("횟수 및 파괴 설정")]
    public int maxUseCount;
    private int currentUseCount = 0;
    public bool destroyAfterInteract = true;

    [Header("상호작용 대화")]
    public List<DialogueLine> eventDialogue;
    
    public void Interact()
    {
        if (currentUseCount >= maxUseCount)
        {
            Debug.Log("더 이상 상호작용할 수 없습니다.");
            return;
        }

        playerRes.ChangeHp(hpChange);
        playerRes.ChangeMp(mpChange);
        playerRes.Exp += expChange;
        playerRes.OnResourceChange?.Invoke();

        currentUseCount++;

        if (dialogueManager != null && eventDialogue.Count > 0)
        {
            dialogueManager.StartDialogue(eventDialogue);
        }

        if (currentUseCount >= maxUseCount) if (dialogueManager != null && eventDialogue.Count > 0)
            {
                if (currentUseCount >= maxUseCount && destroyAfterInteract)
                {
                    dialogueManager.OnDialogueComplete = DestroySelf;
                }
                dialogueManager.StartDialogue(eventDialogue);
            }
    }
    private void DestroySelf()
    {
        Debug.Log($"{gameObject.name}이 대화 종료 후 파괴되었습니다.");
        Destroy(gameObject);
        player.interactIcon.SetActive(false);
    }
}
