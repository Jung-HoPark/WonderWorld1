using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class LevelupManager : MonoBehaviour
{
    [Header("Shared Data")]
    public PlayerResourceManager playerResource;
    public List<CharacterData> partyMembers;

    [Header("UI Panels")]
    public GameObject levelUpCanvas;  // V 누르면 뜨는 전체창
    public GameObject confirmPopup; // "레벨업 하시겠습니까?" 팝업

    [Header("Character Slot UI")]
    public TextMeshProUGUI[] charInfoTexts; // 이름, 레벨, HP, ATK 표시
    public Button[] charButtons;           // 캐릭터 클릭용 버튼

    [Header("Popup UI")]
    public TextMeshProUGUI popupExpText;   // 보유/필요 경험치 표시
    public Button yesButton;               // "예" 버튼

    private CharacterData selectedChar;
    private bool isPanelOpen = false;

    void Update()
    {
        // 1. V키로 열고 닫기
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (!isPanelOpen) OpenMenu();
            else CloseMenu();
        }

        // 2. ESC로 닫기
        if (isPanelOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseMenu();
        }
    }
    public void OpenMenu()
    {
        isPanelOpen = true;
        levelUpCanvas.SetActive(true);
        UpdateUI();
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null) player.SetCanMove(false);
    }
    public void CloseMenu()
    {
        isPanelOpen = false;
        levelUpCanvas.SetActive(false);
        confirmPopup.SetActive(false);
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null) player.SetCanMove(true);
    }
    void UpdateUI()
    {
        for (int i = 0; i < partyMembers.Count; i++)
        {
            if (i < charInfoTexts.Length)
            {
                var data = partyMembers[i];
                charInfoTexts[i].text = $"{data.characterName}\n" +
                                        $"Lv.{data.level}\n" +
                                        $"HP: {data.hp}/{data.maxHp}\n" +
                                        $"ATK: {data.atk}";

                // 버튼 이벤트 연결 (기존 이벤트 삭제 후 재연결)
                int index = i;
                charButtons[i].onClick.RemoveAllListeners();
                charButtons[i].onClick.AddListener(() => OnClickCharacter(partyMembers[index]));
            }
        }
    }
    public void OnClickCharacter(CharacterData data)
    {
        selectedChar = data;
        int cost = data.level * 100; // CharacterData 내의 계산 방식과 동일하게

        popupExpText.text = $"{data.characterName}을(를) 레벨업 하시겠습니까?\n" +
                            $"필요 EXP: {cost} / 보유 EXP: {(int)playerResource.Exp}";

        // 경험치 충분 여부에 따라 '예' 버튼 활성화/비활성화
        yesButton.interactable = (playerResource.Exp >= cost);
        confirmPopup.SetActive(true);
    }
    public void RequestLevelUp()
    {
        if (selectedChar != null)
        {
            // CharacterData 메서드 호출
            selectedChar.LevelUp(playerResource);

            // 레벨업 후 UI 즉시 갱신
            UpdateUI();
            confirmPopup.SetActive(false);

            // 상단 UI바 등을 갱신하기 위한 이벤트 호출
            playerResource.OnResourceChange?.Invoke();
        }
    }
}
