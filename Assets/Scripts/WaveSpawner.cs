using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveSpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public GameObject enemyPrefab;
        public int count;
        public float spawnInterval;
    }

    [Header("Configuración de Oleadas")]
    public List<Wave> waves;
    public Transform[] spawnPoints;
    public float timeBetweenWaves = 5f;

    [Header("Referencias")]
    [SerializeField] private VictoryManager victoryManager;

    private int currentWaveIndex = 0;

    private void Start()
    {
        StartCoroutine(StartNextWave());
    }

    private IEnumerator StartNextWave()
    {
        // 1. Si se completaron todas las oleadas, activa la victoria
        if (currentWaveIndex >= waves.Count)
        {
            Debug.Log("¡Todas las rondas completadas!");
            TriggerVictory();
            yield break;
        }

        // Espera entre rondas
        yield return new WaitForSeconds(timeBetweenWaves);

        Wave currentWave = waves[currentWaveIndex];

        // 2. Spawnea todos los enemigos correspondientes a la ronda
        for (int i = 0; i < currentWave.count; i++)
        {
            SpawnEnemy(currentWave.enemyPrefab);
            yield return new WaitForSeconds(currentWave.spawnInterval);
        }

        // 3. Espera a que no quede NINGÚN enemigo vivo para continuar
        while (GameObject.FindGameObjectsWithTag("Enemy").Length > 0)
        {
            yield return new WaitForSeconds(1f);
        }

        // 4. Incrementa el índice de ronda e inicia la siguiente
        currentWaveIndex++;
        StartCoroutine(StartNextWave());
    }

    private void SpawnEnemy(GameObject enemyPrefab)
    {
        if (spawnPoints == null || spawnPoints.Length == 0) return;

        // Selecciona un spawn point aleatorio
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform selectedPoint = spawnPoints[randomIndex];

        Instantiate(enemyPrefab, selectedPoint.position, Quaternion.identity);
    }

    private void TriggerVictory()
    {
        if (victoryManager != null)
        {
            victoryManager.ShowVictoryScreen();
        }
        else
        {
            FindFirstObjectByType<VictoryManager>()?.ShowVictoryScreen();
        }
    }
}