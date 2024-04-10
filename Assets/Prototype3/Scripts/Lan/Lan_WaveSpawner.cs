using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lan_WaveSpawner : MonoBehaviour
{
    [Header("Unity Set up")]
    [SerializeField] private Transform enemyPrefab;
    [SerializeField] private Transform spawnPoint;

    [Header("Attributes")]
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private float countdown = 2f;
    [SerializeField] private int waveIndex = 0;

    private void Update()
    {
        if (countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
        }

        countdown -= Time.deltaTime;
    }

    IEnumerator SpawnWave()
    {
        
        waveIndex++;
        Debug.Log("Wave Imcoming!");
        for (int i = 0; i < waveIndex; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.5f);
        }

    }

    void SpawnEnemy()
    {
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
