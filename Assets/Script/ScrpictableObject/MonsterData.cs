using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMonster", menuName = "ScriptableObjects/MonsterData")]
public class MonsterData : ScriptableObject
{
    public string monsterName;
    public float hp;
    public float maxHp;
    public float atk;
    public int expReward;
    public bool isDead;

    [Header("Battle Settings")]
    public GameObject modelPrefab;
    public List<ActionData> actionPool;
}
