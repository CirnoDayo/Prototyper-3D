using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lan_EnemyBomb : MonoBehaviour
{
    private Transform target;
    
    //[SerializeField] private float travelSpeed = 70f;
    [SerializeField] float bombExplosionRange = 0f;
    [SerializeField] private GameObject bombShatterEffect;
    [SerializeField] private int damageDealt = 20;

    public void InteractWithAnotherBomb()
    {
       ChainExplode();
       Lan_EnemyScript enemyScript = this.GetComponent<Lan_EnemyScript>();
       enemyScript.TakeDamge(damageDealt);
    }
    void Update()
    {
        
    }

    void ChainExplode()
    {
        GameObject effectInstance = (GameObject)Instantiate(bombShatterEffect, transform.position, transform.rotation);
        Destroy(effectInstance,2f);
        
        Collider[] colldiers = Physics.OverlapSphere(transform.position, bombExplosionRange);
        foreach (Collider collider in colldiers)
        {
            if (collider.CompareTag("NormalEnemy") | collider.CompareTag("EnemyWithBomb"))
            {
                Lan_EnemyScript e = collider.GetComponent<Lan_EnemyScript>();
                if (e != null)
                {
                    e.TakeDamge(damageDealt);
                }
            }
        }
        Destroy(gameObject.GetComponent<Lan_EnemyBomb>());
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, bombExplosionRange);
    }
}
