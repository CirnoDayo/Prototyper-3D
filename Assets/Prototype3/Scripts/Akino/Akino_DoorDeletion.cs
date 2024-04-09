using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Akino_DoorDeletion : MonoBehaviour
{
    public Akino_MapManager mapManager;
    private BoxCollider boxCollider;

    private void Awake()
    {
        mapManager = FindObjectOfType<Akino_MapManager>();
        boxCollider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        if (!Object.ReferenceEquals(this, null) && !boxCollider.isTrigger && mapManager.doorDeleted)
        {
            boxCollider.isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != null && !mapManager.doorDeleted)
        {
            Vector3 direction = transform.localPosition;
            float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
            mapManager.nextTileRotation = Quaternion.Euler(0, angle, 0);
            Destroy(other.gameObject);
            Destroy(this);
            GetComponent<BoxCollider>().enabled = false;
            mapManager.doorDeleted = true;
        }
    }
}
