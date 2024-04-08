using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Akino_DirectionDetectScript : MonoBehaviour
{
    public List<GameObject> doors = new List<GameObject>();
    public LayerMask doorLayer;
    
    private void Start()
    {
        foreach (Transform child in transform)
        {
            if (((1 << child.gameObject.layer) & doorLayer.value) != 0)
            {
                doors.Add(child.gameObject);
            }
        }

        for (int i = 0; i < doors.Count; i++)
        {
            Vector3 direction = doors[i].transform.localPosition;
            float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
            Debug.Log(angle);
        }
    }
}
