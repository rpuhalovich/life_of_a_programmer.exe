using System;
using UnityEngine;

class WallRun
{
    private Transform mainCamera;
    private float minHeight;
    private float maxWallDistance;
    private float wallRunForce;
    private float maxWallRunSpeed;
    private float maxWallRunAngle;
    private int jumpRefresh;
    private LayerMask wallRunable;

    private bool isWallRight, isWallLeft;
    private bool isWallRunning = false;

    public WallRun(Transform mainCamera, float minHeight, float maxWallDistance, float wallRunForce, float maxWallRunSpeed, float maxWallRunAngle, int jumpRefresh, LayerMask wallRunable)
    {
        this.mainCamera = mainCamera;
        this.minHeight = minHeight;
        this.maxWallDistance = maxWallDistance;
        this.wallRunForce = wallRunForce;
        this.maxWallRunSpeed = maxWallRunSpeed;
        this.maxWallRunAngle = maxWallRunAngle;
        this.jumpRefresh = jumpRefresh;
        this.wallRunable = wallRunable;
    }

    public void HandleWallRun(Transform transform, ref Vector3 movementVector, Transform groundCheck, float groundDistance, LayerMask groundMask, KeyCode right, KeyCode left, KeyCode straight)
    {
        // if the player is moving towards a wall and moving forward as well
        if (((Input.GetKey(right) && isWallRight) || (Input.GetKey(left) && isWallLeft)) && Input.GetKey(straight))
        {
            // check if the player is above the minimum height off the ground before starting the wall run
            if (!Physics.Raycast(groundCheck.position, -Vector3.up, groundDistance + minHeight, groundMask))
            {
                StartWallRun(transform, ref movementVector);
            }
        }
        else
        {
            StopWallRun();
        }
    }

    void StartWallRun(Transform transform, ref Vector3 movementVector)
    {
        isWallRunning = true;

        // limit the speed when wall running
        if (movementVector.magnitude <= maxWallRunSpeed)
        {
            // adding forward force
            movementVector += transform.forward * wallRunForce * Time.deltaTime;

            // adding force towards the wall
            if (isWallRight)
            {
                movementVector += transform.right * wallRunForce / 5 * Time.deltaTime;
            }
            else if (isWallLeft)
            {
                movementVector += -transform.right * wallRunForce / 5 * Time.deltaTime;
            }
        }
    }

    void StopWallRun()
    {
        isWallRunning = false;
    }

    public void CheckWalls(Transform transform, ref int numJumped)
    {
        isWallRight = Physics.Raycast(transform.position, transform.right, maxWallDistance, wallRunable);
        isWallLeft = Physics.Raycast(transform.position, -transform.right, maxWallDistance, wallRunable);

        if (!isWallRight && !isWallLeft)
        {
            StopWallRun();
        }

        // reset the number of jumps when wall running
        if (isWallLeft || isWallRight)
        {
            numJumped = jumpRefresh;
        }
    }

    public bool IsWallRunning()
    {
        return isWallRunning;
    }

    public bool IsWallRight()
    {
        return isWallRight;
    }

    public bool IsWallLeft()
    {
        return isWallLeft;
    }
}
