using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;



public class Lan_EnemyScript : MonoBehaviour
{
    [Header("Unity Set up")]
    [SerializeField] Vector3 endPoint; 
    public Lan_LivesUI livesUI;
    public Image healthBar;
    public Lan_WaveSpawner WaveSpawner;
    
    
    
    [Header("Attributes")]
    [SerializeField] NavMeshAgent agent;
    [SerializeField] private int startHealth = 100;
    [SerializeField] private int enemyHealthPoint;
    [SerializeField] private float detectionRange = 5f;
    

    private void Start()
    {
        livesUI = FindObjectOfType<Lan_LivesUI>();
        if (livesUI == null)
        {
            Debug.Log("Can not find Lan_LivesUI");
        }
        endPoint = new Vector3(0,0,0);

        enemyHealthPoint = startHealth;
        healthBar.fillAmount = enemyHealthPoint;
        Lan_EventManager.EnemySpawned();

        WaveSpawner = FindObjectOfType<Lan_WaveSpawner>();

    }

    public void TakeDamge(int amount)
    {
        float defaultHealth = (float)startHealth;
        Debug.Log("Damafe taken:" + amount);
        enemyHealthPoint -= amount;
        healthBar.fillAmount = enemyHealthPoint / defaultHealth;
        if (enemyHealthPoint <= 0)
        {
            Die();
        }
    }
    private void Update()
    {
        agent.SetDestination(endPoint);
        DetectEndPoint();

        if (WaveSpawner.waveIndex > 2)
        {
            agent.speed = 10;
        }
        else if (WaveSpawner.waveIndex > 4)
        {
            agent.speed = 12;
        }
        else if (WaveSpawner.waveIndex > 6)
        {
            agent.speed = 14;
        }
    }
    
    private void DetectEndPoint()
    {
        RaycastHit hit;
        Vector3 direction = endPoint - transform.position;

        
        if (Physics.Raycast(transform.position, direction, out hit, detectionRange))
        {
            
            if (hit.collider.gameObject.transform.position == endPoint)
            {
                
                livesUI.EnemyReachedEnd();
                Die();
            }
        }
    }

    void Die()
    {
        Lan_EventManager.EnemyDestroyed();
        Destroy(gameObject);
    }
}
