using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Lan_WaveSpawner : MonoBehaviour
{
    [Header("Unity Set up")]
    public Transform enemyPrefab;
    public Vector3 spawnPoint;
    public Button startButton;
    public TMPro.TextMeshProUGUI roundCounter;

    [Header("Attributes")] 
    [SerializeField] float delayInstantiateTime = 0f;
    //public float timeBetweenWaves = 5f;
    //public float countdown = 2f;
    [SerializeField] int waveIndex = 0;
   

    [Header("Private")]
    [SerializeField] Akino_MapManager mapManager;

    private void Start()
    {
        mapManager = GetComponent<Akino_MapManager>();
        startButton = GameObject.Find("RoundChangeButton").GetComponent<Button>();
    }

    #region SubscribeToCustomEvent

    private void OnEnable()
    {
        Lan_EventManager.UpdateSpawnedPoint += TheNewSpawnPoint;
    }

    private void OnDisable()
    {
        Lan_EventManager.UpdateSpawnedPoint -= TheNewSpawnPoint;    
    }
    

    #endregion

    private void Update()
    {
        roundCounter.text = waveIndex.ToString();
    }
    private void TheNewSpawnPoint()
    {
        spawnPoint = mapManager.doorDirection.position;
    }
    public void SpawnWaveInput()
    {
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        waveIndex++;
        
        //Debug.Log("Wave Incoming!");
        for (int i = 0; i < waveIndex; i++)
        {
            yield return new WaitForSeconds(delayInstantiateTime);
            SpawnEnemy();
            
        }
    }
    void SpawnEnemy()
    {
        
        Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
    }
}
