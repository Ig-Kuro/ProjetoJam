using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    [Header("Waves")]
    public GameObject[] enemyPrefabs;
    public Transform[] spawnPoints;

    [Header("Configuracoes da Wave")]
    public int currentWave = 0;
    public int enemiesPerWave = 5;
    public float timeBetweenWaves = 10f;
    public float timeBetweenSpawns = 1f;

    private int enemiesSpawned = 0;

    void Start()
    {
        StartCoroutine(StartNextWave());
    }
    IEnumerator StartNextWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);

        currentWave++;
        enemiesSpawned = 0;

        StartCoroutine(SpawnWave());
    }
    IEnumerator SpawnWave()
    {
        while (enemiesSpawned < enemiesPerWave)
        {
            SpawnEnemy();
            enemiesSpawned++;
            yield return new WaitForSeconds(timeBetweenSpawns);
        }

        enemiesPerWave += 2;
        StartCoroutine(StartNextWave());
    }
    void SpawnEnemy()
    {
        int randomSpawnIndex = Random.Range(0, spawnPoints.Length);
        int randomEnemyIndex = Random.Range(0, enemyPrefabs.Length);
        Instantiate(enemyPrefabs[randomEnemyIndex], spawnPoints[randomSpawnIndex].position, Quaternion.identity);
    }
}