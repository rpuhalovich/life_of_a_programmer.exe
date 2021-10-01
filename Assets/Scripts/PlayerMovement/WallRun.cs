using UnityEngine;

class WallRun
{
    private LayerMask wallLayer;
    private bool isWallRight, isWallLeft;
    private bool isWallRunning;

    private void WallRunInput()
    {
        if (Input.GetButtonDown("Right") && isWallRight || Input.GetButtonDown("Left") && isWallLeft)
        {
            StartWallRun();
        }
    }

    private void StartWallRun()
    {

    }

    private void StopWallRun()
    {

    }
}
