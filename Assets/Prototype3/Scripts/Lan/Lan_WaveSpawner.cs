using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Lan_WaveSpawner : MonoBehaviour
{
    [Header("Unity Set up")]
    public Transform enemyPrefab;
    public Vector3 spawnPoint;

    [Header("Attributes")]
    //public float timeBetweenWaves = 5f;
    //public float countdown = 2f;
    public int waveIndex = 0;

    [Header("Private")]
    [SerializeField] Akino_MapManager mapManager;

    private void Start()
    {
        mapManager = GetComponent<Akino_MapManager>();
    }

    private void Update()
    {
        spawnPoint = mapManager.doorDirection.position;
        /*if (countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
        }
        countdown -= Time.deltaTime;*/
    }

    public void SpawnWaveInput()
    {
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        
        waveIndex++;
        Debug.Log("Wave Incoming!");
        for (int i = 0; i < waveIndex; i++)
        {
            yield return new WaitForSeconds(1f);
            SpawnEnemy();
        }

    }

    void SpawnEnemy()
    {
        Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
    }
}
