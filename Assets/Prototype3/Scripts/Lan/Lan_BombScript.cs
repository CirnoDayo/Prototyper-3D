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
    [SerializeField] private int damageDealt = 20;

    public void SeekEnemy(Transform _target)
    {
        target = _target;
    }
    void Update()
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

    void HitTarget()
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
            Damage(target);
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
                Debug.Log("collider name: " + collider.name + " collider tag: " + collider.tag);
                Lan_EnemyScript e = collider.GetComponent<Lan_EnemyScript>();

                if (e != null)
                {
                    e.TakeDamge(damageDealt);
                }
            }
        }
    }

    void Damage(Transform enemy)
    {
        Lan_EnemyScript e = enemy.GetComponent<Lan_EnemyScript>();

        if (e != null)
        {
            e.TakeDamge(damageDealt);
        }
        
        //Destroy(enemy.gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, bombExplosionRange);
    }
}
