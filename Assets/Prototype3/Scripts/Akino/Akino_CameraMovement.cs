using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Akino_CameraMovement : MonoBehaviour
{
    [Range(1f, 5f)] public float sensitivity = 1f;

    Vector3 currentPosition;
    Vector3 movementVector;

    private void Start()
    {
        currentPosition = transform.position;
    }

    private void LateUpdate()
    {
        movementVector.x = Input.GetAxis("Mouse X");
        movementVector.z = Input.GetAxis("Mouse Y");
        movementVector = Quaternion.Euler(0f, 45f, 0f) * movementVector;
        if (Input.GetMouseButton(1))
        {
            transform.position -= movementVector * sensitivity;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
