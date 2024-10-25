using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int enemyCount = 3;
    [SerializeField] private Transform[] spawnPoints;
    public float spawnRadius = 2f; 

    private void Start()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            for (int i = 0; i < enemyCount / spawnPoints.Length; i++)
            {
                SpawnEnemyAtPoint(spawnPoint);
            }
        }
    }

    private void SpawnEnemyAtPoint(Transform spawnPoint)
    {
        Vector2 randomPoint = Random.insideUnitCircle * spawnRadius; 
        Vector3 spawnPosition = new Vector3(randomPoint.x, 0f, randomPoint.y) + spawnPoint.position;

        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}