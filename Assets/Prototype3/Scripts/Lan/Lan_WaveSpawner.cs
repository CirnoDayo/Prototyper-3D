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
    public Transform enemyWithBomb;

    [SerializeField] private int numberOfEnemiesInFirstWave = 10;
    public TMPro.TextMeshProUGUI roundCounter;

    [Header("Attributes")] 
    [SerializeField] float delayInstantiateTime = 0f;
    //public float timeBetweenWaves = 5f;
    //public float countdown = 2f;
    [SerializeField] int waveIndex = 0;
    [SerializeField] private float enemyDefaultInstantiatedRate = .1f;
    [SerializeField] private float firstWaveInsDelayTime = 0.2f;
    [SerializeField] private float increaseInsDelayTimeRate = 0.02f;
   

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
    
    private float numberOfEnemiesLastWave = 0;
    private float numberOfEnemiesThisWave;

    IEnumerator SpawnWave()
    {
        //Debug.Log("SpawnWave method is calling!");
        numberOfEnemiesThisWave = numberOfEnemiesInFirstWave;
        waveIndex++;
        int totalEnemyInstantiated = 0;
        //Debug.Log("waveIndex: " + waveIndex);

        for (int i = 1; i < waveIndex; i++)
        {
            numberOfEnemiesThisWave = numberOfEnemiesLastWave + (numberOfEnemiesLastWave * enemyDefaultInstantiatedRate);
            Debug.Log("For loop for add more enemy is working!");
        }
        
        //Debug.Log("Original numberOfEnemies: " + numberOfEnemiesThisWave);
        //Debug.Log("Round to Int= " + Mathf.RoundToInt(numberOfEnemiesThisWave));
        totalEnemyInstantiated = (int)Mathf.RoundToInt(numberOfEnemiesThisWave);
        
        delayInstantiateTime = firstWaveInsDelayTime - increaseInsDelayTimeRate * (waveIndex - 1);
        Debug.Log("delayInstantiateTime: " + delayInstantiateTime);
        
        for (int i = 0; i < totalEnemyInstantiated; i++)
        {
            yield return new WaitForSeconds(delayInstantiateTime);
            
            SpawnEnemy();
            
        }

        numberOfEnemiesLastWave = numberOfEnemiesThisWave;
        //Debug.Log(numberOfEnemiesLastWave*enemyDefaultInstantiatedRate);



    }
    void SpawnEnemy()
    {
        //Debug.Log("SpawnEnemy is calling!");
        Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
    }
}
