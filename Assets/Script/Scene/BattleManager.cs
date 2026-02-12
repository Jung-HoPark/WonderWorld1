using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    [Header("UI Bars")]
    public Slider[] playerHPBars;
    public Slider monsterHPBar;

    [Header("Shared Resources")]
    public PlayerResourceManager playerResource;
    public Slider teamMpBar;
    public TextMeshProUGUI heartText;

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

    void Start()
    {
        if (playerResource != null)
        {
            playerResource.OnResourceChange += UpdateUIBars;
        }
    }
    void UpdateUIBars()
    {
        // 1. 개별 캐릭터 HP 바
        for (int i = 0; i < playerBattlers.Count; i++)
        {
            if (i < playerHPBars.Length)
            {
                playerHPBars[i].maxValue = playerBattlers[i].maxHp;
                playerHPBars[i].value = playerBattlers[i].hp;
            }
        }
        // 2. 팀 공용 MP 및 하트 UI
        if (playerResource != null)
        {
            teamMpBar.maxValue = playerResource.maxPlayerMp;
            teamMpBar.value = playerResource.playerMp;
            heartText.text = $"x {playerResource.playerHeart}";
        }
        // 3. 몬스터 HP 바
        if (monsterBattler != null)
        {
            monsterHPBar.maxValue = monsterBattler.maxHp;
            monsterHPBar.value = monsterBattler.hp;
        }
    }
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
        UpdateUIBars();

        GenerateMonsterActions();
        StartSelectionPhase();
    }
    public void StartExecuteTurn()
    {
        StartCoroutine(ExecuteTurnCoroutine());
    }
    IEnumerator ExecuteTurnCoroutine()
    {
        commandPanel.SetActive(false);
        startBattleButton.SetActive(false);

        for (int i = 0; i < 3; i++)
        {
            if (i < playerActions.Count && i < monsterActions.Count)
            {
                ActionData pAct = playerActions[i];
                ActionData mAct = monsterActions[i];

                if (pAct != null && mAct != null)
                {
                    characterNameText.text = $"{playerBattlers[i].name} VS 몬스터!";
                    Debug.Log($"<color=cyan>[{i + 1}번 공방]</color> {playerBattlers[i].name}({pAct.actionName}) VS 몬스터({mAct.actionName})");

                    yield return new WaitForSeconds(0.5f);

                    CompareAndAct(i, pAct, mAct);

                    yield return new WaitForSeconds(1.2f);
                }
            }

            if (monsterBattler.isDead) break;
        }

        if (monsterBattler.isDead)
        {
            WinBattle();
        }
        else
        {
            ResetTurn();
        }
    }
    public void ResetTurn()
    {
        currentSelectionIndex = 0;
        playerActions.Clear();
        for (int i = 0; i < 3; i++) playerActions.Add(null);

        foreach (var slot in selectionSlots)
        {
            if (slot != null) slot.gameObject.SetActive(false);
        }

        GenerateMonsterActions();
        UpdateCharacterSelectionUI();

        commandPanel.SetActive(true);
        startBattleButton.SetActive(false);

        Debug.Log("새로운 턴");
    }
    void CompareAndAct(int playerIndex, ActionData pAction, ActionData mAction)
    {
        // 1. 최종 데미지 계산 (atk + damageValue)
        int pFinalDmg = Mathf.RoundToInt(playerBattlers[playerIndex].atk + pAction.damageValue);
        int mFinalDmg = Mathf.RoundToInt(monsterBattler.atk + mAction.damageValue);

        // 2. 판정
        if (pFinalDmg > mFinalDmg)
        {
            ApplyDamageToMonster(pFinalDmg);
            Debug.Log($"<color=green>성공!</color> {playerBattlers[playerIndex].name}의 압승! ({pFinalDmg} vs {mFinalDmg})");
        }
        else if (mFinalDmg > pFinalDmg)
        {
            ApplyDamageToPlayer(mFinalDmg);
            Debug.Log($"<color=red>실패!</color> 몬스터의 반격! ({mFinalDmg} vs {pFinalDmg})");
        }
        else
        {
            // 무승부 (0 데미지 연출)
            ShowDamagePopup(monsterSpawnPoint.position + Vector3.up * 1.5f, 0);
            ShowDamagePopup(playerSpawnPoints[playerIndex].position + Vector3.up * 1.5f, 0);
            Debug.Log("<color=yellow>상쇄!</color> 두 공격의 위력이 같아 무효화되었습니다.");
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
        UpdateUIBars();

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
            if (target.hp <= 0)
            {
                // PRM의 CanRevive() 사용 (하트 > 0 인지 체크)
                if (playerResource.CanRevive())
                {
                    playerResource.ChangeHp(-1); // 하트 1개 감소
                    target.hp = Mathf.RoundToInt(target.maxHp * 0.5f); // 50% 부활
                    target.isDead = false;

                    Debug.Log($"<color=yellow>{target.name} 부활!</color> 하트 사용.");
                    ShowDamagePopup(playerSpawnPoints[targetIdx].position + Vector3.up * 1.5f, 777); // 부활 연출용
                }
                else
                {
                    target.isDead = true;
                    playerSpawnPoints[targetIdx].gameObject.SetActive(false); // 캐릭터 숨기기
                    Debug.Log($"{target.name} 쓰러짐...");
                }
            }
            UpdateUIBars();
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

                var intentScript = intentIcons[i].GetComponent<MonsterIntent>();
                if (intentScript != null)
                {
                    intentScript.currentIntentAction = monsterActions[i];
                }
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

        if (playerResource != null)
        {
            playerResource.ChangeMp(-selectedAction.manaCost);
        }

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

                        bool canAfford = playerResource.playerMp >= data.skillList[i].manaCost;
                        commandButtons[i].GetComponent<Button>().interactable = canAfford;
                    }
                    else
                    {
                        commandButtons[i].gameObject.SetActive(false); // 스킬이 적으면 버튼 숨김
                    }
                }
            }
        }
    }
    public void CancelLastSelection()
    {
        if (currentSelectionIndex <= 0) return;
        currentSelectionIndex--;

        // 1. 소모했던 MP 복구
        ActionData canceledAction = playerActions[currentSelectionIndex];
        if (canceledAction != null && playerResource != null)
        {
            playerResource.ChangeMp(canceledAction.manaCost); // 다시 더해줌
        }

        playerActions[currentSelectionIndex] = null;

        selectionSlots[currentSelectionIndex].gameObject.SetActive(false);
        UpdateCharacterSelectionUI();

        startBattleButton.SetActive(false);
        commandPanel.SetActive(true);
    }
    public void ResetSelection()
    {
        currentSelectionIndex = 0;
        playerActions.Clear();
        for (int i = 0; i < 3; i++) playerActions.Add(null);
    }
    void WinBattle()
    {
        Debug.Log("<color=yellow>전투 승리!</color>");
        int rewardExp = 1000;
        if (playerResource != null)
        {
            playerResource.Exp += rewardExp;
            Debug.Log($"{rewardExp} EXP 획득! 현재 총 EXP: {playerResource.Exp}");
        }

        Invoke("ReturnToField", 2.0f);
    }
    void ReturnToField()
    {
        FinishBattle();
        SceneManager.LoadScene("Stage2");
    }
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
