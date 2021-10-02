using UnityEngine;

public class BoostPad
{
    private float boostAmt;
    private LayerMask boostPadLayer;

    public BoostPad(float boostAmt, LayerMask boostPadLayer)
    {
        this.boostAmt = boostAmt;
        this.boostPadLayer = boostPadLayer;
    }

    public void HandleBoost(Transform transform, float playerHeight, ref float velocityY)
    {
        // 8 is boost layer.
        if (Physics.Raycast(transform.position, -transform.up, playerHeight, boostPadLayer))
        {
            velocityY = boostAmt;
        }
    }
}
