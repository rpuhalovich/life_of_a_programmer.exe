using System;
using UnityEngine;

public class Grapple
{
    private Camera playerCamera;
    private GrappleFOV fov;
    private Transform grappleLine;
    public LayerMask grappleable;
    private float maxGrappleDist;

    private Vector3 grapplePos;
    private float grappleLineSize;
    private grappleState state;
    public enum grappleState
    {
        normal, shoot, launch
    }

    public Grapple(Camera playerCamera, GrappleFOV fov, Transform grappleLine, LayerMask grappleable, float maxGrappleDist)
    {
        this.fov = fov;
        this.playerCamera = playerCamera;
        this.grappleLine = grappleLine;
        this.grappleable = grappleable;
        this.maxGrappleDist = maxGrappleDist;
        this.state = grappleState.normal;
    }

    public bool HandleGrappleStart()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // if there is a 'grappleable' object within the max grapple distance and the first object hit from the raycast is in the grapple layer, then proceed
            if ((Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit rayHit, maxGrappleDist)) && ((rayHit.transform.gameObject.layer == LayerMask.NameToLayer("Grapple"))))
            {
                // sets grapple position, allows the grapple line to be seen, and initiates the grapple launch state
                //grapplePos = rayHit.point;
                grapplePos = rayHit.collider.bounds.center;
                grappleLineSize = 0.0f;
                grappleLine.gameObject.SetActive(true);
                grappleLine.localScale = Vector3.zero;
                state = grappleState.shoot;
                return true;
            }
        }
        return false;
    }

    // has the player shoot out the grappling line
    public void HandleGrappleShoot(Transform transform, float GRAPPLE_FOV)
    {
        grappleLine.LookAt(grapplePos);

        float shootSpeed = 150.0f;
        // elongates the grappling line
        grappleLineSize += shootSpeed * Time.deltaTime;
        grappleLine.localScale = new Vector3(0.1f, 0.1f, grappleLineSize);

        // when the grappling line becomes more than the distance to the grapple position, change state
        if (grappleLineSize >= Vector3.Distance(transform.position, grapplePos))
        {
            state = grappleState.launch;
            fov.SetFOV(GRAPPLE_FOV);
        }
    }

    // launches the player towards the grapple position
    public void HandleGrappleLaunch(CharacterController controller, Transform transform, ref Vector3 velocity, ref Vector3 velocityMomentum, float jumpHeight, float NORMAL_FOV)
    {
        grappleLine.LookAt(grapplePos);
        Vector3 grappleDir = (grapplePos - transform.position).normalized;

        float minLaunchSpeed = 25.0f;
        float launchSpeed = 2.0f * Math.Max(Vector3.Distance(transform.position, grapplePos), minLaunchSpeed);
        controller.Move(grappleDir * launchSpeed * Time.deltaTime);

        // used to indicate if centre of player is within this distance of the grapple position
        float reachedGrapplePos = 2.0f;
        // if player reaches the end of the grapple position
        if (Vector3.Distance(transform.position, grapplePos) < reachedGrapplePos)
        {
            float momentumSpeed = 0.15f;
            velocityMomentum = grappleDir * launchSpeed * momentumSpeed;
            velocityMomentum += Vector3.up * jumpHeight;
            ResetGrapple(ref state, ref velocity, ref grappleLine, NORMAL_FOV);
        }

        // cancel the grapple mid way
        if (Input.GetMouseButtonDown(0))
        {
            ResetGrapple(ref state, ref velocity, ref grappleLine, NORMAL_FOV);
        }

        // cancel the grapple mid way with a jump
        /*if (Input.GetButtonDown("Jump")) {
            float momentumSpeed = 0.15f;
            velocityMomentum = grappleDir * launchSpeed * momentumSpeed;
            velocityMomentum += Vector3.up * jumpHeight;
            ResetGrapple(ref state, ref velocity, ref grappleLine, NORMAL_FOV);
        }*/
    }

    void ResetGravity(ref Vector3 velocity)
    {
        velocity.y = -2.0f;
    }

    // resets the grapple state, gravity after the grapple, and doesn't show the grapple line
    void ResetGrapple(ref grappleState state, ref Vector3 velocity, ref Transform grappleLine, float NORMAL_FOV)
    {
        state = grappleState.normal;
        ResetGravity(ref velocity);
        grappleLine.gameObject.SetActive(false);
        fov.SetFOV(NORMAL_FOV);
    }

    // returns the current grapple state
    public grappleState State()
    {
        return this.state;
    }
}
