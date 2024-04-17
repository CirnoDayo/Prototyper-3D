using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lan_TowerScript : MonoBehaviour
{
    [Header("Unity Set up")]
    public Image healthBar;
    
    
    
    [Header("Attributes")]
    [SerializeField] private int startHealth = 100;
    [SerializeField] private int towerHealthPoint;

    private void Start()
    {
        towerHealthPoint = startHealth;
    }

    public void TowerTakeDamge(int amount)
    {
        
        Debug.Log("Damage taken:" + amount);
        float defaultHealth = (float)startHealth;
        towerHealthPoint -= amount;
        
        healthBar.fillAmount = towerHealthPoint / defaultHealth;
        if (towerHealthPoint <= 0)
        {
            TowerDestroy();
        }
    }
  
    void TowerDestroy()
    {
        Destroy(gameObject);
    }
}
