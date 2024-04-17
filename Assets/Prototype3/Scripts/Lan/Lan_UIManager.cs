using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Lan_UIManager : MonoBehaviour
{
    [SerializeField] int activeEnemies = 0;
    [SerializeField] private int destroyedAmount = 0;
    [SerializeField] CannonManager cannonManager;
    [SerializeField] private Lan_WaveSpawner _lanWaveSpawner;
    [SerializeField] private Lan_LivesUI livesUI;
    public Button startButton;
    [SerializeField] private bool enemiesSpawned;

    private void Start()
    {
        startButton = GameObject.Find("RoundChangeButton").GetComponent<Button>();
        cannonManager = GetComponent<CannonManager>();
        _lanWaveSpawner = GetComponent<Lan_WaveSpawner>();
        livesUI = GetComponent<Lan_LivesUI>();
    }

    #region SbscribeToCustomEvent

    private void OnEnable()
    {
        Lan_EventManager.OnEnemySpawned += EnemySpawned;
        Lan_EventManager.OnEnemyDestroyed += EnemyDestroyed;
    }

    private void OnDisable()
    {
        Lan_EventManager.OnEnemySpawned -= EnemySpawned;
        Lan_EventManager.OnEnemyDestroyed -= EnemyDestroyed;
    }
    

    #endregion

    private void AddNewTower()
    {
        if (destroyedAmount == Mathf.RoundToInt(_lanWaveSpawner.numberOfEnemiesThisWave))
        {
            destroyedAmount = 0;
            Lan_EventManager.AddingNewTowerRandomly();
            cannonManager.UpdateUI();
        }
        else
        {
            return;
        }
    }

    private void EnemySpawned()
    {
        activeEnemies++;
        

    }

    private void EnemyDestroyed()
    {
        activeEnemies--;
        destroyedAmount++;
        if(activeEnemies <= 0 && livesUI.currentPlayersHP >= 0)
        {
            
            startButton.interactable = true;
           
            AddNewTower();
        }
    }
        
}
