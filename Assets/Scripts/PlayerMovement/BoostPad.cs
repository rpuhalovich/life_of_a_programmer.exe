using UnityEngine;

public class BoostPad
{
    private float boostAmt;

    public BoostPad(float boostAmt)
    {
        this.boostAmt = boostAmt;
    }

    public void HandleBoost(ref float velocityY)
    {
        velocityY = boostAmt;
    }
}
