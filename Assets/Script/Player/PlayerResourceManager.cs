using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
[CreateAssetMenu(fileName="PlayerResource",menuName= "ScriptableObjects/PlayerResource")]
public class PlayerResourceManager : ScriptableObject
{
    [Header("Shared Resources")]
    public int playerHp;
    public int maxPlayerHp;
    public float playerMp;
    public float maxPlayerMp;
    public int Exp;

    public Action OnResourceChange;

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
