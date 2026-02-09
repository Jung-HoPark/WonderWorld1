using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "ScriptableObjects/CharacterData")]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public int level;
    public float hp;
    public float atk;
    public int amount;  // 레벨업 시 hp 증가량
    public int amount2;  // 레벨업 시 atk 증가량
    public float maxHp;
    public bool isDead;

    [Header("Battle Settings")]
    public GameObject modelPrefab;
    public List<ActionData> skillList;

    public void LevelUp(PlayerResourceManager resource)
    {
        int cost = level * 100;
        if (resource.Exp >= cost)
        {
            resource.Exp -= cost;
            level++;
            maxHp += amount;
            hp = maxHp;
            atk += amount2;
            Debug.Log($"{characterName} 레벨업! 현재 레벨: {level}");
        }
    }
}
