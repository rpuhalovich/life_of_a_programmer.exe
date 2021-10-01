using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleFOV : MonoBehaviour
{
    private Camera playerCamera;
    private float grappleFOV;
    private float currentFOV;

    private void Awake()
    {
        playerCamera = GetComponent<Camera>();
        grappleFOV = playerCamera.fieldOfView;
        currentFOV = grappleFOV;
    }

    private void Update()
    {
        float fovSpeed = 5.0f;
        currentFOV = Mathf.Lerp(currentFOV, grappleFOV, Time.deltaTime * fovSpeed);
        playerCamera.fieldOfView = currentFOV;
    }

    public void SetFOV(float grappleFOV)
    {
        this.grappleFOV = grappleFOV;
    }
}
