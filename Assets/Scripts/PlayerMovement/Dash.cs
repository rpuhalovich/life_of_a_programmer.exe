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
    private bool cooldownRefreshed = false;

    public Dash(float dashSpeed, float dashLength, float dashResetTime, int maxDashAttempts)
    {
        this.dashSpeed = dashSpeed;
        this.dashLength = dashLength;
        this.dashResetTime = dashResetTime;
        this.maxDashAttempts = maxDashAttempts;
    }

    public void HandleDash(Vector3 movementVector, Transform transform, CharacterController characterController, ref float velocityY)
    {
        //bool isTryingToDash = Input.GetButtonDown("Dash") || Input.GetButtonDown("Crouch"); // TODO: Not great putting input code here...
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
            velocityY = 0.0f;

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

        ResetDashAttempts();
    }

    void ResetDashAttempts()
    {
        if (Time.time - lastResetTime >= dashResetTime)
        {
            cooldownRefreshed = true;
            lastResetTime = Time.time;
            dashAttempts = Mathf.Max(0, dashAttempts - 1);
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

    public float GetCooldown()
    {
        return dashResetTime;
    }

    public int GetDashAttempts()
    {
        return dashAttempts;
    }

    public int GetMaxDashAttempts()
    {
        return maxDashAttempts;
    }

    public bool IsDashing()
    {
        return isDashing;
    }

    public float CurrCooldownTime()
    {
        return Time.time - lastResetTime;
    }

    public bool CooldownRefreshed()
    {
        return cooldownRefreshed;
    }

    public void ResetCooldown()
    {
        cooldownRefreshed = false;
    }
}
