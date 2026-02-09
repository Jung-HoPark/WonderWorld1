using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
[CreateAssetMenu(fileName="PlayerResource",menuName= "ScriptableObjects/PlayerResource")]
public class PlayerResourceManager : ScriptableObject
{
    [Header("Shared Resources")]
    public int playerHp;
    public float playerMp;
    public int Exp;

    [Header("Default Settings")]
    public int maxPlayerHp;
    public int initPlayerHp;
    public float maxPlayerMp;
    public float initPlayerMp;

    public Action OnResourceChange;

    public void ResetData()
    {
        playerHp = initPlayerHp;
        playerMp = initPlayerMp;
        Exp = 0;
        // UI에 초기화된 값을 알림
        OnResourceChange?.Invoke();
    }
    public void ChangeHp(int amount)
    {
        playerHp = Mathf.Clamp(playerHp + amount, 0, maxPlayerHp);
        OnResourceChange?.Invoke();
    }

    public void ChangeMp(float amount)
    {
        playerMp = Mathf.Clamp(playerMp + amount, 0, maxPlayerMp);
        OnResourceChange?.Invoke();
    }

    public bool CanRevive() => playerHp > 0;
    public bool CanEnhanceSkill(float cost) => playerMp >= cost;

    public void Initialize()
    {
        playerHp = maxPlayerHp;
        playerMp = (float)(maxPlayerMp * 0.5);
        Exp = 0;
    }
}
