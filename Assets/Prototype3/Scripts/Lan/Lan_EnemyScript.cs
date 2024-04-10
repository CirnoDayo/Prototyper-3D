using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lan_EnemyScript : MonoBehaviour
{
    [Header("Unity Set up")]
    [SerializeField] Transform endPoint; 
    private Lan_LivesUI livesUI; 
    
    [Header("Attributes")]
    [SerializeField] private int enemyHealthPoint = 100;
    [SerializeField] private float detectionRange = 5f; 


    private void Start()
    {
        livesUI = FindObjectOfType<Lan_LivesUI>();
        if (livesUI == null)
        {
            Debug.Log("Can not find Lan_LivesUI");
        }
        endPoint = GameObject.Find("Lan_EndPath").transform;
        
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
        DetectEndPoint();
        
    }
    
    private void DetectEndPoint()
    {
        RaycastHit hit;
        Vector3 direction = endPoint.position - transform.position;

       
        if (Physics.Raycast(transform.position, direction, out hit, detectionRange))
        {
            
            if (hit.collider.gameObject == endPoint.gameObject)
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
