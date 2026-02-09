using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSpawner : MonoBehaviour
{
    [Header("References")]
    public BattleManager battleManager;

    [Header("Spawn Points")]
    public Transform[] playerSpawnPoints;
    public Transform monsterSpawnPoint;

    [Header("Data to Spawn")]
    public List<CharacterData> partyToSpawn;
    public MonsterData monsterToSpawn;
    void Start()
    {
        SpawnModels();

        if (battleManager != null)
        {
            battleManager.SetupBattle(partyToSpawn, monsterToSpawn);
            battleManager.GenerateMonsterActions();
        }
    }
    void SpawnModels()
    {
        for (int i = 0; i < partyToSpawn.Count; i++)
        {
            if (i < playerSpawnPoints.Length && partyToSpawn[i].modelPrefab != null)
            {
                Instantiate(partyToSpawn[i].modelPrefab, playerSpawnPoints[i].position, playerSpawnPoints[i].rotation);
            }
        }

        if (monsterToSpawn != null && monsterToSpawn.modelPrefab != null)
        {
            Instantiate(monsterToSpawn.modelPrefab, monsterSpawnPoint.position, monsterSpawnPoint.rotation);
        }
    }
}
