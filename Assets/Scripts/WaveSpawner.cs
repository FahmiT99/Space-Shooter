using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public GameObject enemyPrefab1;
    public GameObject enemyPrefab2;
    public GameObject enemyPrefab3;
    public GameObject enemyPrefab4;
    public GameObject enemyPrefab5;
    public GameObject bossPrefab1;
    public GameObject bossPrefab2;

    public WaveCountUI waveCountUI;

    public int enemiesPerWave = 5;
    private float timeBetweenWaves = 2f;

    private float waveTimer;
    private int currentWave = 0;

    private void Start()
    {
        waveTimer = timeBetweenWaves;
    }

    private void Update()
    {
        if (!HasActiveEnemies())
        {
            if (waveTimer <= 0f)
            {
                SpawnWave();
                waveTimer = timeBetweenWaves;
                currentWave++;
            }
            else
            {
                waveTimer -= Time.deltaTime;
            }
        }
    }


    private void SpawnWave()
    {
        if ((currentWave + 1) % 5 == 0)
        {
            // Spawn a boss wave
            int bossCount = Random.Range(1, 3); // Randomly determine the number of bosses (1 or 2)
            for (int i = 0; i < bossCount; i++)
            {
                float randomY = Random.Range(-4f, 4f);
                float randomX = Random.Range(4f, 8f); // Adjust the range as needed for the desired spawn positions
                Vector3 spawnPosition = new Vector3(randomX, randomY, 0f);
                GameObject bossPrefab = GetRandomBossPrefab();
                if (bossPrefab == bossPrefab2)
                {
                    // Rotate bossPrefab2 by 90 degrees
                    Instantiate(bossPrefab, spawnPosition, Quaternion.Euler(0f, 0f, 90f));
                }
                else
                {
                    Instantiate(bossPrefab, spawnPosition, Quaternion.identity);
                }
            }
        }
        else
        {
            // Spawn a regular wave
            for (int i = 0; i < enemiesPerWave; i++)
            {
                float randomY = Random.Range(-4f, 4f);
                float randomX = Random.Range(4f, 8f); // Adjust the range as needed for the desired spawn positions
                Vector3 spawnPosition = new Vector3(randomX, randomY, 0f);
                GameObject enemyPrefab = GetRandomEnemyPrefab();
                if (i < enemiesPerWave)
                {
                    Instantiate(enemyPrefab, spawnPosition, Quaternion.Euler(0f, 0f, -90f));
                }
            }
        }
        waveCountUI.IncreaseWaveCount();
    }




    private GameObject GetRandomEnemyPrefab()
    {
        // Select a random index within the range of regular enemy prefabs
        int randomIndex = Random.Range(1, 6);
        switch (randomIndex)
        {
            case 1:
                return enemyPrefab1;
            case 2:
                return enemyPrefab2;
            case 3:
                return enemyPrefab3;
            case 4:
                return enemyPrefab4;
            case 5:
                return enemyPrefab5;
            default:
                return null;
        }
    }

    private GameObject GetRandomBossPrefab()
    {
        // Select a random boss enemy prefab
        int randomIndex = Random.Range(1, 3);
        switch (randomIndex)
        {
            case 1:
                return bossPrefab1;
            case 2:
                return bossPrefab2;
            default:
                return null;
        }
    }

    private bool HasActiveEnemies()
    {
        // Check if there are any active enemies or boss enemies in the scene
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] bossEnemies = GameObject.FindGameObjectsWithTag("EnemyBoss1");
        return enemies.Length > 0 || bossEnemies.Length > 0;
    }
}
