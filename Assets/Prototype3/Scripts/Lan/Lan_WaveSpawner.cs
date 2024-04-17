using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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
        mapManager.RandomEntrance();
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
    private int enemyWithBombExist = 0;
    private int normalEnemyExist = 0;

    
    IEnumerator SpawnWave()
    {
        
        numberOfEnemiesThisWave = numberOfEnemiesInFirstWave;
        waveIndex++;
        int totalEnemyInstantiated = 0;
        int enemyWithBombsCount = 0;
        int normalEnemyCount = 0;
        normalEnemyExist = 0;
        enemyWithBombExist = 0;

        if (2 < waveIndex && waveIndex < 5)
        {
            bombRatio = 0.3f;
        }
        else if (waveIndex > 4)
        {
            bombRatio = 0.2f;
        }

        for (int i = 1; i < waveIndex; i++)
        {
            numberOfEnemiesThisWave = numberOfEnemiesLastWave + (numberOfEnemiesLastWave * enemyDefaultInstantiatedRate);
        }
        
        totalEnemyInstantiated = (int)Mathf.RoundToInt(numberOfEnemiesThisWave);
        
        enemyWithBombsCount = (int)Mathf.RoundToInt(totalEnemyInstantiated * bombRatio);
        
        normalEnemyCount = totalEnemyInstantiated - enemyWithBombsCount;
        

        delayInstantiateTime = firstWaveInsDelayTime - increaseInsDelayTimeRate * (waveIndex - 1);

        yield return new WaitForSeconds(3f);
        for (int i = 0; i < totalEnemyInstantiated;)
        {

            int randomNumber = Random.Range(0, 2); // Random integer between 0 (inclusive) and 2 (exclusive)
            //Debug.Log("randomNumber: " + randomNumber);
            bool conditionChecked = false;
            
            while (!conditionChecked)
            {
                if (randomNumber == 0)
                {
                    if (normalEnemyExist >= normalEnemyCount)
                    {
                        randomNumber = 1;  
                    }
                    else
                    {
                        SpawnEnemy(0);
                        normalEnemyExist++;
                        conditionChecked = true; 
                    }
                }
                else if (randomNumber == 1)
                {
                    
                    if (enemyWithBombExist >= enemyWithBombsCount)
                    {
                        randomNumber = 0;  
                    }
                    else
                    {
                        SpawnEnemy(1);
                        enemyWithBombExist++;
                        conditionChecked = true; 
                    }
                }
            }
            i++;
            yield return new WaitForSeconds(delayInstantiateTime);
        }
        numberOfEnemiesLastWave = numberOfEnemiesThisWave;
    }
    void SpawnEnemy(int enemyIndex)
    {
        mapManager.RandomEntrance();
        spawnPoint = mapManager.doorDirection.position;
        
        Quaternion rotation = Quaternion.Euler(0, 180, 0);
        Instantiate(enemyPrefab[enemyIndex], spawnPoint, rotation);
    }

    public bool divisible()
    {
        if (waveIndex == 1)
        {
            return true;
        }
        return waveIndex % 2 == 0;
    }
}
