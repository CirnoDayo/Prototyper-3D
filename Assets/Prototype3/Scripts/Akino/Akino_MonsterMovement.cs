using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Akino_MonsterMovement : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform target;
    private void Update()
    {
        agent.SetDestination(target.transform.position);
    }
}
