using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Lan_EnemyScript : MonoBehaviour
{
    [Header("Unity Set up")]
    [SerializeField] Vector3 endPoint; 
    public Lan_LivesUI livesUI; 
    
    [Header("Attributes")]
    [SerializeField] NavMeshAgent agent;
    [SerializeField] private int enemyHealthPoint = 100;
    [SerializeField] private float detectionRange = 5f;

    private void Start()
    {
        livesUI = FindObjectOfType<Lan_LivesUI>();
        if (livesUI == null)
        {
            Debug.Log("Can not find Lan_LivesUI");
        }
        endPoint = new Vector3(0,0,0);
    }

    public void TakeDamge(int amount)
    {
        enemyHealthPoint = enemyHealthPoint - amount;
        Debug.Log("Enemy health left: " + enemyHealthPoint);
        if (enemyHealthPoint <= 0)
        {
            Die();
        }
    }
    private void Update()
    {
        agent.SetDestination(endPoint);
        DetectEndPoint();
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
                Destroy(gameObject);
            }
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
