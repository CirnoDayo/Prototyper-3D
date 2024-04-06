using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lan_BombScript : MonoBehaviour
{
    private Transform target;
    [SerializeField] private float travelSpeed = 70f;
    [SerializeField] private GameObject bombShatterEffect;

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
        
    }

    void HitTarget()
    {
        //When hit enemy, instantiate the effect so players know that they hit the enemy.
        GameObject effectInstance = (GameObject)Instantiate(bombShatterEffect, transform.position, transform.rotation);
        Destroy(effectInstance,2f);
        Destroy(target.gameObject);
        Destroy(gameObject);
    }
}
