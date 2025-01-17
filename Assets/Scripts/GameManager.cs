using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    [Header("Spawn Settings")]
    [SerializeField] private List<GameObject> enemyPrefabs;
    [SerializeField] private Transform player;
    [SerializeField] private float initialSpawnInterval = 3f;
    [SerializeField] private int maxEnemies = 50;
    [SerializeField] private List<float> spawnRates;
    [SerializeField] private float spawnRadius = 10f;
    [SerializeField] private float minSpawnDistance = 3f;

    [Header("Wave Settings")]
    [SerializeField] private Text waveText;
    [SerializeField] private Text killCounterText;
    [SerializeField] private float waveDuration = 10f;

    public List<GameObject> activeEnemies = new List<GameObject>();
    private float spawnTimer = 0f;
    private float waveTimer = 0f;
    private float currentSpawnInterval;
    private float totalSpawnRate;

    private int currentWave = 1;
    private int killCount = 0;

    protected override void Awake()
    {
        base.Awake();
        //DontDestroyOnLoad(gameObject);

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

        currentSpawnInterval = initialSpawnInterval;
        UpdateUI();
    }

    void Update()
    {
        waveTimer += Time.deltaTime;
        spawnTimer += Time.deltaTime;

        CleanUpDeadEnemies();

        if (waveTimer >= waveDuration)
        {
            StartNextWave();
        }

        if (spawnTimer >= currentSpawnInterval && activeEnemies.Count < maxEnemies)
        {
            SpawnEnemy();
            spawnTimer = 0f;
        }
    }

    private void CleanUpDeadEnemies()
    {
        for (int i = activeEnemies.Count - 1; i >= 0; i--)
        {
            if (activeEnemies[i] == null)
            {
                activeEnemies.RemoveAt(i);
                killCount++;
                UpdateUI();
            }
        }
    }


    private void StartNextWave()
    {
        waveTimer = 0f;
        currentWave++;
        currentSpawnInterval *= 0.75f;
        UpdateUI();
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
        if (player != null)
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

        return Vector3.zero;
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

    public void EnemyKilled(GameObject enemy)
    {
        if (activeEnemies.Contains(enemy))
        {
            activeEnemies.Remove(enemy);
            Destroy(enemy);
            killCount++;
            UpdateUI();
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

    private void UpdateUI()
    {
        if (waveText != null)
        {
            waveText.text = "Wave: " + currentWave;
        }

        if (killCounterText != null)
        {
            killCounterText.text = "Kills: " + killCount;
        }
    }

    public void ResetGame()
    {
        currentWave = 1;
        killCount = 0;

        spawnTimer = 0f;
        waveTimer = 0f;
        currentSpawnInterval = initialSpawnInterval;

        RemoveAllEnemies();

        UpdateUI();
    }

}
