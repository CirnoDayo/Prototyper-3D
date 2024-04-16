using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Lan_EnemyBomb : MonoBehaviour
{
    private Transform target;
    
    //[SerializeField] private float travelSpeed = 70f;
    [SerializeField] float bombExplosionRange = 10f;
    [SerializeField] private GameObject bombShatterEffect;
    [SerializeField] private int damageDealt = 20;
    [SerializeField] private bool hasExploded = false; 

    public void InteractWithAnotherBomb()
    {
        if (hasExploded) return; // Exit if this bomb has already ex
        hasExploded = true; // Set the flag to prevent further explosions
        #region EffectInstantiate
        GameObject effectInstance = (GameObject)Instantiate(bombShatterEffect, transform.position, transform.rotation);
        Destroy(effectInstance,2f);
        #endregion
        ChainExplode();
        
    }
    
    void ChainExplode()
    {
       
        ApplyDamage(gameObject.transform, damageDealt);
        
        List<Collider> filteredColliders = new List<Collider>();
        Collider[] colliders = Physics.OverlapSphere(transform.position, bombExplosionRange);
        foreach (Collider collider in colliders)//Filter itself
        {
            if (collider.transform != transform)
            {
                filteredColliders.Add(collider);
            }
        }
        
        foreach (Collider filteredCollider in filteredColliders) 
        {
            if (filteredCollider.CompareTag("NormalEnemy"))//Filter the none enemy one
            {
                ApplyDamage(filteredCollider.transform,damageDealt/2);
            }
        }
        //Destroy(gameObject.GetComponent<Lan_EnemyBomb>());
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, bombExplosionRange);
    }
}
