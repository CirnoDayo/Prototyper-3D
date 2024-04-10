using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Lan_HeavyCannonScript : MonoBehaviour
{
    private Transform target;
    
    [Header("Attributes")]
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float fireCountdown = 0f;
    [SerializeField] private float range = 15f;
    [SerializeField] private float headTurningSpeed = 10f;
    
    [Header("Unity Setup Fields")]
    [SerializeField] string enemyTag = "EnemyWithBomb";
    [SerializeField] Transform partToRotate;
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private Transform firePoint;   
    

    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        
        //Find enemy -> detect if they are in range -> compare to find the nearest -> choose target
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }
    void Update()
    {
        if (target == null)
        {
            return;
        }

        //Get the Head to rotate to enemy in range
        Vector3 direction = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation,lookRotation, Time.deltaTime * headTurningSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y,0f);

        //Cannon countdown
        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }
        fireCountdown -= Time.deltaTime;

    }

    void Shoot()
    {
        GameObject bombGO = (GameObject) Instantiate(bombPrefab, firePoint.position, firePoint.rotation);
        Lan_BombScript bomb = bombGO.GetComponent<Lan_BombScript>();
        

        if (bomb != null)
        {
            bomb.SeekEnemy(target);
        }
    }

    private void OnDrawGizmosSelected() //Just color hehe
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
