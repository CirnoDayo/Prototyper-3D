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
       
        Lan_EnemyScript enemyScript = gameObject.GetComponent<Lan_EnemyScript>();
        if (enemyScript != null)
        {
            enemyScript.TakeDamge(damageDealt);
        }
        
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
                Lan_EnemyScript e = filteredCollider.GetComponent<Lan_EnemyScript>();
                Debug.Log(e);
                Lan_EnemyBomb enemyBomb = filteredCollider.GetComponent<Lan_EnemyBomb>();
                if (e != null)
                {
                    e.TakeDamge(damageDealt/2);
                    
                }

                if (enemyBomb != null)
                {
                    enemyBomb.InteractWithAnotherBomb();
                }
                else
                {
                    return;
                }
            }
        }
        Destroy(gameObject.GetComponent<Lan_EnemyBomb>());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, bombExplosionRange);
    }
}
