using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;

    private float xRotation = 0f;

    private int cursorModeCount;
    void Start()
    {
        cursorModeCount = (int)CursorLockMode.Locked;
        Cursor.lockState = (CursorLockMode)cursorModeCount;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        playerBody.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            cursorModeCount++;
            cursorModeCount = cursorModeCount == 3 ? 0 : cursorModeCount;
            Cursor.lockState = (CursorLockMode)cursorModeCount;
        }
    }
}
