using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Akino_DoorDeletion : MonoBehaviour
{
    public Akino_MapManager mapManager;
    private BoxCollider boxCollider;
    private bool touchedGrass;

    private void Awake()
    {
        mapManager = FindObjectOfType<Akino_MapManager>();
        boxCollider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        if (!boxCollider.isTrigger && mapManager.doorDeleted)
        {
            mapManager.doorDirection = transform;
            boxCollider.isTrigger = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            touchedGrass = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.layer != LayerMask.NameToLayer("Ground")) && !touchedGrass)
        {
            Vector3 direction = transform.localPosition;
            Destroy(other.gameObject);
            Destroy(this);
            mapManager.doorDeleted = true;
        }
    }
}
