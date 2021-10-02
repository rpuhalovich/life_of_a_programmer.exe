using UnityEngine;

public class BoostPad
{
    private float boostAmt;
    private float boostDelay;

    public BoostPad(float boostAmt, float boostDelay)
    {
        this.boostAmt = boostAmt;
        this.boostDelay = boostDelay;
    }

    public void HandleBoost(ref float velocityY)
    {
        velocityY = boostAmt;
    }
}
