using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook
{
    private Transform mainCamera;
    private float mouseSensitivity;
    private float maxWallRunAngle;

    private float xRotation;
    private float currWallRunAngle;

    private float rotateSpeedMultiplier;

    public MouseLook(Transform mainCamera, float mouseSensitivity, float maxWallRunAngle, float rotateSpeedMultiplier)
    {
        this.mainCamera = mainCamera;
        this.mouseSensitivity = mouseSensitivity;
        this.maxWallRunAngle = maxWallRunAngle;
        this.currWallRunAngle = 0.0f;
        this.xRotation = 0.0f;

        this.rotateSpeedMultiplier = rotateSpeedMultiplier;
    }

    // Start is called before the first frame update
    public void MouseStart()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    public void HandleMouse(Transform transform, bool isWallRunning, bool isWallRight, bool isWallLeft)
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        mainCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, currWallRunAngle);
        transform.Rotate(Vector3.up * mouseX);

        // rotate the camera anti-clockwise if player is wall running on a wall to their right, capping at the maxWallRunAngle
        if ((Math.Abs(currWallRunAngle) < maxWallRunAngle) && isWallRight && isWallRunning)
        {
            currWallRunAngle += maxWallRunAngle * Time.deltaTime * rotateSpeedMultiplier;
        }
        // rotate the camera clockwise if a player is wall running on a wall to their left, capping at the maxWallRunAngle
        if ((Math.Abs(currWallRunAngle) < maxWallRunAngle) && isWallLeft && isWallRunning)
        {
            currWallRunAngle -= maxWallRunAngle * Time.deltaTime * rotateSpeedMultiplier;
        }

        // reverts the camera angle back to normal
        if ((currWallRunAngle > 0) && !isWallRight && !isWallLeft)
        {
            currWallRunAngle -= maxWallRunAngle * Time.deltaTime * rotateSpeedMultiplier;
        }
        if ((currWallRunAngle < 0) && !isWallRight && !isWallLeft)
        {
            currWallRunAngle += maxWallRunAngle * Time.deltaTime * rotateSpeedMultiplier;
        }
    }
}
