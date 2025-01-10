using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("Spawn Settings")]
    [SerializeField] private List<GameObject> enemyPrefabs;
    [SerializeField] private Transform player;
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private int maxEnemies = 50;
    [SerializeField] private List<float> spawnRates;
    [SerializeField] private float spawnRadius = 10f;
    [SerializeField] private float minSpawnDistance = 3f;

    public List<GameObject> activeEnemies = new List<GameObject>();
    private float spawnTimer = 0f;
    private float totalSpawnRate;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);

        if (enemyPrefabs.Count != spawnRates.Count)
        {
            Debug.LogError("The number of enemy prefabs does not match the number of spawn rates.");
            return;
        }

        totalSpawnRate = 0f;
        foreach (float rate in spawnRates)
        {
            totalSpawnRate += rate;
        }
    }


    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval && activeEnemies.Count < maxEnemies)
        {
            SpawnEnemy();
            spawnTimer = 0f;
        }
    }

    private void SpawnEnemy()
    {
        GameObject selectedEnemy = GetRandomEnemyPrefab();
        Vector3 spawnPosition = GetRandomSpawnPositionAroundPlayer();

        GameObject newEnemy = Instantiate(selectedEnemy, spawnPosition, Quaternion.identity);
        activeEnemies.Add(newEnemy);
    }

    private Vector3 GetRandomSpawnPositionAroundPlayer()
    {
        Vector3 spawnPosition;
        do
        {
            float angle = Random.Range(0f, 360f);
            float distance = Random.Range(minSpawnDistance, spawnRadius);
            Vector3 offset = new Vector3(Mathf.Cos(angle) * distance, 0, Mathf.Sin(angle) * distance);
            spawnPosition = player.position + offset;
        }
        while (Vector3.Distance(spawnPosition, player.position) < minSpawnDistance);

        return spawnPosition;
    }

    private GameObject GetRandomEnemyPrefab()
    {
        float randomValue = Random.Range(0, totalSpawnRate);
        float cumulativeRate = 0f;

        for (int i = 0; i < enemyPrefabs.Count; i++)
        {
            cumulativeRate += spawnRates[i];
            if (randomValue <= cumulativeRate)
            {
                return enemyPrefabs[i];
            }
        }

        return enemyPrefabs[0];
    }

    private void RemoveEnemy(GameObject enemy)
    {
        if (activeEnemies.Contains(enemy))
        {
            activeEnemies.Remove(enemy);
            Destroy(enemy);
        }
    }

    public void RemoveAllEnemies()
    {
        foreach (GameObject enemy in activeEnemies)
        {
            if (enemy != null) Destroy(enemy);
        }
        activeEnemies.Clear();
    }
}
