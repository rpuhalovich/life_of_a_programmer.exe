using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashUI
{
    [SerializeField] private Image arrow1;
    [SerializeField] private Image arrow2;
    [SerializeField] private Image arrow3;
    [SerializeField] private Image dash1;
    [SerializeField] private Image dash2;
    private Dash dash;
    private float cooldown;
    private float maxDashAttempts;
    private bool isCooldown = false;

    public DashUI(Dash dash, Image arrow1, Image arrow2, Image arrow3, Image dash1, Image dash2)
    {
        this.dash = dash;
        this.arrow1 = arrow1;
        this.arrow2 = arrow2;
        this.arrow3 = arrow3;
        this.dash1 = dash1;
        this.dash2 = dash2;
        this.cooldown = dash.GetCooldown();
        this.maxDashAttempts = dash.GetMaxDashAttempts();

        // initialise the fill amounts
        this.arrow1.fillAmount = 0;
        this.arrow2.fillAmount = 0;
        this.arrow3.fillAmount = 0;
        this.dash1.fillAmount = 0;
        this.dash2.fillAmount = 0;
    }

    // Update is called once per frame
    public void HandleDashUI()
    {
        if (Input.GetButtonDown("Dash") && dash.IsDashing())
        {
            isCooldown = true;
            arrow1.fillAmount = 1;
            arrow2.fillAmount = 1;
            arrow3.fillAmount = 1;

            if (dash.GetDashAttempts() == 1)
            {
                dash2.fillAmount = 1;
            }
            if (dash.GetDashAttempts() == 2)
            {
                dash1.fillAmount = 1;
            }
        }

        if (isCooldown)
        {
            arrow1.fillAmount = 1 - dash.CurrCooldownTime() / cooldown;
            arrow2.fillAmount = 1 - dash.CurrCooldownTime() / cooldown;
            arrow3.fillAmount = 1 - dash.CurrCooldownTime() / cooldown;

            if (dash.CooldownRefreshed())
            {    
                dash.ResetCooldown();
                if (dash.GetDashAttempts() == 0)
                {
                    arrow1.fillAmount = 0;
                    arrow2.fillAmount = 0;
                    arrow3.fillAmount = 0;

                    dash2.fillAmount = 0;
                    isCooldown = false;
                } else if (dash.GetDashAttempts() == 1)
                {
                    arrow1.fillAmount = 1;
                    arrow2.fillAmount = 1;
                    arrow3.fillAmount = 1;

                    dash1.fillAmount = 0;
                }
            }
        }
    }
}
