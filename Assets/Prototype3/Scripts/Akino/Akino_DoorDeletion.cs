using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Akino_DoorDeletion : MonoBehaviour
{
    Akino_MapManager mapManager;
    private void Start()
    {
        mapManager = GetComponent<Akino_MapManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("yep");
        //mapManager.doorDeleted = true;
        //Destroy(other.gameObject);
        //Destroy(gameObject);
    }
}
