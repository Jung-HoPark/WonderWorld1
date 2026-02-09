using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    [Header("Runtime Data")]
    public List<BattleEntity> playerBattlers = new List<BattleEntity>();
    public BattleEntity monsterBattler;

    [Header("Turn Actions")]
    public List<ActionData> playerActions = new List<ActionData>();
    public List<ActionData> monsterActions = new List<ActionData>();

    [Header("Monster UI")]
    public Image[] intentIcons;

    [Header("Selection UI")]
    public TextMeshProUGUI characterNameText;
    public GameObject commandPanel;
    public GameObject startBattleButton;
    public Image[] selectionSlots;

    [Header("Spawn Points (Ref)")]
    public Transform monsterSpawnPoint;
    public Transform[] playerSpawnPoints;

    [Header("Skill Buttons")]
    public ActionButton[] commandButtons;

    [Header("Effects")]
    public GameObject damagePopupPrefab;
    public Transform monsterUIAnchor;
    public Transform[] playerUIAnchors;

    private int currentSelectionIndex = 0;

    public void SetupBattle(List<CharacterData> party, MonsterData monster)
    {
        playerBattlers.Clear();
        foreach (var data in party)
        {
            playerBattlers.Add(new BattleEntity(data));
        }

        if (monster != null)
        {
            monsterBattler = new BattleEntity(monster);
            Debug.Log($"몬스터 {monsterBattler.name} 데이터 복사 완료");
            StartSelectionPhase();
        }
        else
        {
            Debug.LogError("전달받은 몬스터 데이터가 Null입니다!");
        }
    }
    public void ExecuteTurn()
    {
        for (int i = 0; i < 3; i++)
        {
            if (i < playerActions.Count && i < monsterActions.Count)
            {
                if (playerActions[i] != null && monsterActions[i] != null)
                {
                    CompareAndAct(i, playerActions[i], monsterActions[i]);
                }
            }
        }
    }
    int CalculateFinalDamage(BattleEntity attacker, ActionData action)
    {
        return Mathf.RoundToInt(attacker.atk + action.damageValue);
    }
    void CompareAndAct(int playerIndex, ActionData pAction, ActionData mAction)
    {
        // 1. 플레이어 최종 데미지 (공격력 * 스킬계수)
        int playerFinalDmg = CalculateFinalDamage(playerBattlers[playerIndex], pAction);

        // 2. 몬스터 최종 데미지
        int monsterFinalDmg = CalculateFinalDamage(monsterBattler, mAction);

        // 3. 판정 및 실행 (데미지가 높은 쪽만 공격 성공)
        if (playerFinalDmg > monsterFinalDmg)
        {
            ApplyDamageToMonster(playerFinalDmg);
            Debug.Log($"{playerBattlers[playerIndex].name} 승리! {playerFinalDmg} 데미지 부여");
        }
        else if (monsterFinalDmg > playerFinalDmg)
        {
            ApplyDamageToPlayer(monsterFinalDmg);
            Debug.Log($"몬스터 승리! {monsterFinalDmg} 데미지 피격");
        }
        else
        {
            Debug.Log($"{playerIndex + 1}번째 공방: 서로의 공격이 상쇄되었습니다.");
        }
    }
    void ApplyDamageToMonster(int damage)
    {
        monsterBattler.TakeDamage(damage);
        if (monsterSpawnPoint != null)
        {
            ShowDamagePopup(monsterSpawnPoint.position + Vector3.up * 1.5f, damage);
        }
        Debug.Log($"{monsterBattler.name} HP: {monsterBattler.hp}");

        if (monsterBattler.isDead) WinBattle();
    }
    void ApplyDamageToPlayer(int damage)
    {
        var aliveOnes = playerBattlers.FindAll(b => !b.isDead);
        if (aliveOnes.Count > 0)
        {
            var target = aliveOnes[Random.Range(0, aliveOnes.Count)];
            target.TakeDamage(damage);

            int targetIdx = playerBattlers.IndexOf(target);

            if (targetIdx >= 0 && targetIdx < playerSpawnPoints.Length)
            {
                ShowDamagePopup(playerSpawnPoints[targetIdx].position + Vector3.up * 1.5f, damage);
            }
        }
    }
    void ShowDamagePopup(Vector3 worldPosition, int damage)
    {
        GameObject popup = Instantiate(damagePopupPrefab);
        Canvas mainCanvas = FindObjectOfType<Canvas>();
        if (mainCanvas != null)
        {
            // 두 번째 인자를 'false'로 해야 프리팹에 설정한 크기가 유지됩니다.
            popup.transform.SetParent(mainCanvas.transform, false);
            Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPosition);
            screenPos.z = 0;
            popup.transform.position = screenPos;
            popup.GetComponent<DamagePopup>().Setup(damage);
            popup.transform.localScale = Vector3.one;
        }
    }
    public void GenerateMonsterActions()
    {
        monsterActions.Clear();

        MonsterData origin = monsterBattler.originData as MonsterData;

        if (origin != null && origin.actionPool.Count > 0)
        {
            for (int i = 0; i < 3; i++)
            {
                int randomIndex = Random.Range(0, origin.actionPool.Count);
                monsterActions.Add(origin.actionPool[randomIndex]);
            }
        }

        UpdateMonsterIntentUI();
    }
    void UpdateMonsterIntentUI()
    {
        for (int i = 0; i < monsterActions.Count; i++)
        {
            if (i < intentIcons.Length)
            {
                intentIcons[i].sprite = monsterActions[i].actionIcon; // ActionData에 등록한 아이콘
                intentIcons[i].gameObject.SetActive(true);
            }
        }
    }
    public void StartSelectionPhase()
    {
        currentSelectionIndex = 0;
        playerActions.Clear();
        for (int i = 0; i < 3; i++) playerActions.Add(null);

        startBattleButton.SetActive(false);
        commandPanel.SetActive(true);

        UpdateCharacterSelectionUI();
    }
    public void SelectPlayerAction(ActionData selectedAction)
    {
        if (currentSelectionIndex >= 3) return;

        playerActions[currentSelectionIndex] = selectedAction;
        Debug.Log($"{playerBattlers[currentSelectionIndex].name} : {selectedAction.actionName} 예약");

        currentSelectionIndex++;

        if (currentSelectionIndex < 3)
        {
            UpdateCharacterSelectionUI();
        }
        else
        {
            commandPanel.SetActive(false);
            startBattleButton.SetActive(true);
            characterNameText.text = "전투시작";
        }

        UpdateSelectionUI();
    }
    void UpdateSelectionUI()
    {
        for (int i = 0; i < currentSelectionIndex; i++)
        {
            selectionSlots[i].sprite = playerActions[i].actionIcon;
            selectionSlots[i].gameObject.SetActive(true);
        }
    }
    void UpdateCharacterSelectionUI()
    {
        if (currentSelectionIndex < playerBattlers.Count)
        {
            BattleEntity currentHero = playerBattlers[currentSelectionIndex];
            characterNameText.text = $"{currentHero.name}의 차례";

            // 캐릭터 데이터(SO)에서 스킬 리스트를 가져와 버튼에 주입
            CharacterData data = currentHero.originData as CharacterData;
            if (data != null && data.skillList.Count > 0)
            {
                for (int i = 0; i < commandButtons.Length; i++)
                {
                    if (i < data.skillList.Count)
                    {
                        commandButtons[i].gameObject.SetActive(true);
                        commandButtons[i].SetAction(data.skillList[i]); // 버튼의 데이터 교체
                    }
                    else
                    {
                        commandButtons[i].gameObject.SetActive(false); // 스킬이 적으면 버튼 숨김
                    }
                }
            }
        }
    }
    public void ResetSelection()
    {
        currentSelectionIndex = 0;
        playerActions.Clear();
        for (int i = 0; i < 3; i++) playerActions.Add(null);
    }
    // --- 미구현 함수들 (에러 방지용) ---
    void WinBattle() { Debug.Log("전투에서 승리했습니다!"); FinishBattle(); }
    // ----------------------------------------------
    public void FinishBattle()
    {
        foreach (var battler in playerBattlers)
        {
            // 참조하고 있던 원본 SO를 가져와서 최종 HP 업데이트
            CharacterData origin = battler.originData as CharacterData;
            if (origin != null)
            {
                origin.hp = battler.hp;
                origin.isDead = battler.isDead;
            }
        }
        Debug.Log("데이터 동기화 완료.");
    }
}
