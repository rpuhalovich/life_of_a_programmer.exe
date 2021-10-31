using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair
{
    private Camera playerCamera;
    private float maxGrappleDist;
    private LayerMask grappleable;
    private Transform isGrappleable;
    private Transform isntGrappleable;

    public Crosshair(Camera playerCamera, float maxGrappleDist, LayerMask grappleable, Transform isGrappleable, Transform isntGrappleable)
    {
        this.playerCamera = playerCamera;
        this.maxGrappleDist = maxGrappleDist;
        this.grappleable = grappleable;
        this.isGrappleable = isGrappleable;
        this.isntGrappleable = isntGrappleable;
    }

    // function changes the crosshair to distinguish if player is able to execute the grapple move
    public void checkGrappleableCrosshair()
    {
        if ((Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit rayHit, maxGrappleDist)) && (rayHit.transform.gameObject.layer == LayerMask.NameToLayer("Grapple")))
        {
            isGrappleable.gameObject.SetActive(true);
            isntGrappleable.gameObject.SetActive(false);
        }
        else
        {
            isGrappleable.gameObject.SetActive(false);
            isntGrappleable.gameObject.SetActive(true);
        }
    }
}
