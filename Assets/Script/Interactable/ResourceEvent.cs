using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceEvent : MonoBehaviour, IInteractable
{
    public PlayerResourceManager playerRes;
    public DialogueManager dialogueManager;
    public PlayerController player;
    private bool isInteracting = false;

    [Header("자원 변경량")]
    public int hpChange;
    public float mpChange;
    public int expChange;

    [Header("횟수 및 파괴 설정")]
    public int maxUseCount;
    private int currentUseCount;
    public bool destroyAfterInteract = true;

    [Header("상호작용 대화")]
    public List<DialogueLine> eventDialogue;
    
    void Start()
    {
        currentUseCount = 0;
    }
    public void Interact()
    {
        if (isInteracting || currentUseCount >= maxUseCount) return;

        isInteracting = true;

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
            // 대화가 끝날 때 실행될 함수 등록
            // 기존 OnDialogueComplete에 추가로 'isInteracting = false'를 해줘야 함
            dialogueManager.OnDialogueComplete = () => {
                isInteracting = false; // 이제 다시 상호작용 가능
                if (currentUseCount >= maxUseCount && destroyAfterInteract)
                {
                    DestroySelf();
                }
            };

            dialogueManager.StartDialogue(eventDialogue);
        }
        else
        {
            isInteracting = false;
        }
    }
    private void DestroySelf()
    {
        Debug.Log($"{gameObject.name}이 대화 종료 후 파괴되었습니다.");
        Destroy(gameObject);
        player.interactIcon.SetActive(false);
    }
}
