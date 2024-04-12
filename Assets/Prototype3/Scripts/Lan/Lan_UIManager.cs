using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Lan_UIManager : MonoBehaviour
{
    [SerializeField] int activeEnemies = 0;
    [SerializeField] CannonManager cannonManager;
    public Button startButton;

    private void Start()
    {
        startButton = GameObject.Find("RoundChangeButton").GetComponent<Button>();
        cannonManager = GetComponent<CannonManager>();
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

    private void EnemySpawned()
    {
        activeEnemies++;
        
    }

    private void EnemyDestroyed()
    {
        activeEnemies--;
        if(activeEnemies <= 0)
        {
            startButton.interactable = true;
            cannonManager.UpdateUI();
        }
    }
        
}
