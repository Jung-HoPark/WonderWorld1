using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BattleEntity
{
    public string name;
    public float hp;
    public float maxHp;
    public float atk;
    public bool isDead;

    public ScriptableObject originData;

    public BattleEntity(CharacterData data)
    {
        name = data.characterName;
        maxHp = data.maxHp;
        hp = data.hp;
        atk = data.atk;
        isDead = data.isDead;
        originData = data;
    }
    public BattleEntity(MonsterData data)
    {
        if (data == null) return;

        name = data.monsterName;
        maxHp = data.maxHp;
        hp = data.hp;
        atk = data.atk;
        isDead = data.isDead;
        originData = data;
    }
    public void TakeDamage(int amount)
    {
        hp -= amount;
        hp = Mathf.Clamp(hp, 0, maxHp);
        if (hp <= 0) isDead = true;
    }
}
