using UnityEngine;

public class Dash
{
    private float dashSpeed;
    private int maxDashAttempts;
    private float dashLength; // Amount of time to dash for.
    private float dashResetTime; // Amount of time before you can dash again.

    private bool isDashing;
    private int dashAttempts;
    private float dashStartTime;
    private float lastResetTime;

    public Dash(float dashSpeed, float dashLength, float dashResetTime, int maxDashAttempts)
    {
        this.dashSpeed = dashSpeed;
        this.dashLength = dashLength;
        this.dashResetTime = dashResetTime;
        this.maxDashAttempts = maxDashAttempts;
    }

    public void HandleDash(Vector3 movementVector, Transform transform, CharacterController characterController, bool isGrounded)
    {
        bool isTryingToDash = Input.GetButtonDown("Dash");

        if (isTryingToDash && !isDashing)
        {
            if(dashAttempts < maxDashAttempts)
            {
                OnStartDash();
            }
        }

        if (isDashing)
        {
            // If the dashtime hasn't elapsed.
            if (Time.time - dashStartTime <= dashLength)
            {
                // If the player isn't inputting anything.
                if (movementVector.Equals(Vector3.zero))
                {
                    Vector3 trans = transform.forward.normalized;
                    trans.y = 0.0f;
                    characterController.Move(trans * dashSpeed * Time.deltaTime);
                }
                else
                {
                    Vector3 move = movementVector.normalized;
                    move.y = 0.0f;
                    characterController.Move(move * dashSpeed * Time.deltaTime);
                }
            }
            else
            {
                OnEndDash();
            }
        }

        ResetDashAttempts(isGrounded); // Dash can only be reset on the ground.
    }

    void ResetDashAttempts(bool isGrounded)
    {
        if (!isGrounded) return;
        if (Time.time - lastResetTime >= dashResetTime)
        {
            lastResetTime = Time.time;
            dashAttempts = 0;
        }
    }

    void OnStartDash()
    {
        isDashing = true;
        dashStartTime = Time.time;
        dashAttempts += 1;
    }

    void OnEndDash()
    {
        isDashing = false;
        dashStartTime = 0;
    }
}
