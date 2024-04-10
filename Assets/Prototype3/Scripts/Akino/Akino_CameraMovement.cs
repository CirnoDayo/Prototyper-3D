using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Akino_CameraMovement : MonoBehaviour
{
    public float minZoom = 5f;
    public float maxZoom = 50f;
    [Range(0, 20)] public int sensitivity = 10;

    Vector3 currentPosition;
    Vector3 movementVector;

    private void Start()
    {
        currentPosition = transform.position;
    }

    private void Update()
    {
        movementVector.x = Input.GetAxis("Mouse X");
        movementVector.z = Input.GetAxis("Mouse Y");
        movementVector = Quaternion.Euler(0f, 45f, 0f) * movementVector;
        if (Input.GetMouseButton(1))
        {
            transform.position += movementVector * (Camera.main.orthographicSize / 5);
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        float size = Camera.main.orthographicSize - scroll * sensitivity;
        size = Mathf.Clamp(size, minZoom, maxZoom);
        Camera.main.orthographicSize = size;
    }
}
