using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Lan_WaveSpawner : MonoBehaviour
{
    #region Variables
    [Header("Unity Set up")]
    public Transform[] enemyPrefab;
    public Vector3 spawnPoint;
    public Button startButton;
    public TMPro.TextMeshProUGUI roundCounter;
    [Header("Attributes")]
    [Range(0f, 1f)] public float bombRatio;
    public int numberOfEnemiesInFirstWave = 10;
    public float enemyDefaultInstantiatedRate;
    public float firstWaveInsDelayTime;
    public float increaseInsDelayTimeRate;
    public float delayInstantiateTime;
    public int waveIndex = 0;
    [Header("Private")]
    [SerializeField] Akino_MapManager mapManager;
    #endregion

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
    public float numberOfEnemiesThisWave;
    public float normalEnemyCountThisWave;
    public float enemyWithBombsCountThisWave;
    public int a; public int b;

    IEnumerator SpawnWave()
    {
        numberOfEnemiesThisWave = numberOfEnemiesInFirstWave;
        waveIndex++;
        int totalEnemyInstantiated = 0;
        int enemyWithBombsCount = 0;
        int normalEnemyCount = 0;

        for (int i = 1; i < waveIndex; i++)
        {
            numberOfEnemiesThisWave = numberOfEnemiesLastWave + (numberOfEnemiesLastWave * enemyDefaultInstantiatedRate);
        }
        totalEnemyInstantiated = (int)Mathf.RoundToInt(numberOfEnemiesThisWave);
        a = enemyWithBombsCount = (int)Mathf.RoundToInt(totalEnemyInstantiated * bombRatio);
        b = normalEnemyCount = totalEnemyInstantiated - enemyWithBombsCount;

        delayInstantiateTime = firstWaveInsDelayTime - increaseInsDelayTimeRate * (waveIndex - 1);

        yield return new WaitForSeconds(1f);
        for (int i = 0; i < totalEnemyInstantiated;)
        {
            Debug.Log("!");
            yield return new WaitForSeconds(delayInstantiateTime);
            if(normalEnemyCountThisWave <= normalEnemyCount)
            {
                SpawnEnemy(0);
                normalEnemyCountThisWave++;
                i++;
            }
            else if (enemyWithBombsCountThisWave <= enemyWithBombsCount)
            {
                SpawnEnemy(1);
                enemyWithBombsCount++;
                i++;
            }
        }
        numberOfEnemiesLastWave = numberOfEnemiesThisWave;
    }
    void SpawnEnemy(int enemyIndex)
    {
        Instantiate(enemyPrefab[enemyIndex], spawnPoint, Quaternion.identity);
    }
}
