using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
[CreateAssetMenu(fileName="PlayerResource",menuName= "ScriptableObjects/PlayerResource")]
public class PlayerResourceManager : ScriptableObject
{
    [Header("Shared Resources")]
    public int playerHeart;
    public float playerMp;
    public int Exp;

    [Header("Default Settings")]
    public int maxPlayerHeart;
    public int initPlayerHeart;
    public float maxPlayerMp;
    public float initPlayerMp;

    public Action OnResourceChange;

    public void ResetData()
    {
        playerHeart = initPlayerHeart;
        playerMp = initPlayerMp;
        Exp = 0;
        // UI에 초기화된 값을 알림
        OnResourceChange?.Invoke();
    }
    public void ChangeHp(int amount)
    {
        playerHeart = Mathf.Clamp(playerHeart + amount, 0, maxPlayerHeart);
        OnResourceChange?.Invoke();
    }

    public void ChangeMp(float amount)
    {
        playerMp = Mathf.Clamp(playerMp + amount, 0, maxPlayerMp);
        OnResourceChange?.Invoke();
    }

    public bool CanRevive() => playerHeart > 0;
    public bool CanEnhanceSkill(float cost) => playerMp >= cost;

    public void Initialize()
    {
        playerHeart = maxPlayerHeart;
        playerMp = (float)(maxPlayerMp * 0.5);
        Exp = 0;
    }
}
