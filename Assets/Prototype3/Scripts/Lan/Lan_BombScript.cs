using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lan_BombScript : MonoBehaviour
{
    private Transform target;
    
    [SerializeField] private float travelSpeed = 70f;
    [SerializeField] float bombExplosionRange = 0;
    [SerializeField] private GameObject bombShatterEffect;
    [SerializeField] private int damageDealt = 0;
    [SerializeField] private bool hasExploded = false; 

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
        
        
        if (bombExplosionRange > 0f)
        {
            
            if (hasExploded) return;
            hasExploded = true;
            GameObject effectInstance = (GameObject)Instantiate(bombShatterEffect, transform.position, transform.rotation);
            Destroy(effectInstance,2f);
            Explode();

        }
        else
        {
            DamageEnemy(target);
            Destroy(gameObject);
        }
        
    }

    void Explode()
    {
        ApplyDamage(target, damageDealt);
        
        List<Collider> filteredColliders = new List<Collider>();
        Collider[] colliders = Physics.OverlapSphere(transform.position, bombExplosionRange);
        foreach (Collider collider in colliders)//Filter itself
        {
            if (collider.transform != target.transform)
            {
                filteredColliders.Add(collider);
            }
        }
        
        foreach (Collider filteredCollider in filteredColliders)
        { 
            if (filteredCollider.CompareTag("NormalEnemy"))
            {
                ApplyDamage(filteredCollider.transform,damageDealt/2);
            }
        }
        
        Destroy(gameObject);
    }

    void ApplyDamage(Transform enemy, int damage)
    {
        Lan_EnemyScript e = enemy.GetComponent<Lan_EnemyScript>();
        if (e != null)
        {
            e.TakeDamge(damage);
                   
        }
        Lan_EnemyBomb enemyBomb = enemy.GetComponent<Lan_EnemyBomb>();
        if (enemyBomb != null)
        {
            enemyBomb.InteractWithAnotherBomb();
        }
        else
        {
            return;
        }
    }

    void DamageEnemy(Transform enemy)
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
        else
        {
            return;
        }
        
    }
    

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, bombExplosionRange);
    }
}
