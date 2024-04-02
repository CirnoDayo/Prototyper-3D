using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Akino_CameraMovement : MonoBehaviour
{
    [Range(1f, 5f)] public float sensitivity = 2f;
    public Vector3 mouseInput;

    private void FixedUpdate()
    {
        mouseInput.x = -Input.GetAxis("Mouse X");
        mouseInput.z = -Input.GetAxis("Mouse Y");
        Quaternion rotation = transform.rotation;
        if (Input.GetMouseButton(1))
        {
            Cursor.lockState = CursorLockMode.Locked;
            mouseInput = rotation * mouseInput.normalized;
            mouseInput.y = 0f;
            transform.position += mouseInput * sensitivity;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
