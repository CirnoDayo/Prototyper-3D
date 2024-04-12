using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lan_BombScript : MonoBehaviour
{
    private Transform target;
    
    [SerializeField] private float travelSpeed = 70f;
    [SerializeField] float bombExplosionRange = 0f;
    [SerializeField] private GameObject bombShatterEffect;
    [SerializeField] private int damageDealt = 0;

    public void SeekEnemy(Transform _target)
    {
        target = _target;
    }
    void Update()//Enemy movement
    {
        //Destroy when there is no enemy
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }
        Vector3 direction = target.position - transform.position;
        float distanceThisFrame = travelSpeed * Time.deltaTime;
        
        //Compare the bomb distance and enemy
        if (direction.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }
        //The bomb movement.
        transform.Translate(direction.normalized * distanceThisFrame, Space.World);
        transform.LookAt(target);
    }

    void HitTarget() //Bullet Hit target detector
    {
        //When hit enemy, instantiate the effect so players know that they hit the enemy.
        GameObject effectInstance = (GameObject)Instantiate(bombShatterEffect, transform.position, transform.rotation);
        Destroy(effectInstance,2f);
        
        if (bombExplosionRange > 0f)
        {
            Explode();
            
        }
        else
        {
            DamageEnemy(target);
            
        }
        
        Destroy(gameObject);
    }

    void Explode()
    {
        Collider[] colldiers = Physics.OverlapSphere(transform.position, bombExplosionRange);
        foreach (Collider collider in colldiers)
        {
            
            if (collider.CompareTag("NormalEnemy")) 
            {
                
                Lan_EnemyScript e = collider.GetComponent<Lan_EnemyScript>();
                if (e != null)
                {
                    e.TakeDamge(damageDealt);
                }
                Debug.Log(e.ToString());
            }
            else if (collider.CompareTag("EnemyWithBomb"))
            {
                Lan_EnemyScript e = collider.GetComponent<Lan_EnemyScript>();
                if (e != null)
                {
                    e.TakeDamge(damageDealt);
                }
                Lan_EnemyBomb enemyBomb = collider.GetComponent<Lan_EnemyBomb>();
                if (enemyBomb != null)
                {
                    enemyBomb.InteractWithAnotherBomb();
                }
            }
        }
    }

    void DamageEnemy(Transform enemy)
    {
      
        if (enemy.CompareTag("NormalEnemy"))
        {

            Lan_EnemyScript e = enemy.GetComponent<Lan_EnemyScript>();
            if (e != null)
            {
                e.TakeDamge(damageDealt);
            }
           
        }
        else if (enemy.CompareTag("EnemyWithBomb"))
        {
            Lan_EnemyScript e = enemy.GetComponent<Lan_EnemyScript>();
            if (e != null)
            {
                e.TakeDamge(damageDealt);
            }
            Lan_EnemyBomb enemyBomb = enemy.GetComponent<Lan_EnemyBomb>();
            if (enemyBomb != null)
            {
                enemyBomb.InteractWithAnotherBomb();
            }
        }

        //Destroy(enemy.gameObject);
    }

    void Destroy()
    {
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, bombExplosionRange);
    }
}
