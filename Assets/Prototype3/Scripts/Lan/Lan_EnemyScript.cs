using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lan_EnemyScript : MonoBehaviour
{
    [SerializeField] private int enemyHealthPoint = 100;

    public void TakeDamge(int amount)
    {
        enemyHealthPoint = enemyHealthPoint - amount;
        Debug.Log("Enemy health left: " + enemyHealthPoint);
        if (enemyHealthPoint <= 0)
        {
            Die();
        }
    }

   

    void Die()
    {
        Destroy(gameObject);
    }
}
