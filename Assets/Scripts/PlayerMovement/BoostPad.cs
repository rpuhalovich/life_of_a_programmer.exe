using UnityEngine;

public class BoostPad
{
    private float boostYAmt;
    private float boostForwardAmt;
    private LayerMask boostPadLayer;

    public BoostPad(float boostYAmt, float boostForwardAmt, LayerMask boostPadLayer)
    {
        this.boostYAmt = boostYAmt;
        this.boostForwardAmt = boostForwardAmt;
        this.boostPadLayer = boostPadLayer;
    }

    public void HandleBoost(Transform transform, float playerHeight, ref float velocityY)
    {
        // 8 is boost layer.
        if (Physics.Raycast(transform.position, -transform.up, playerHeight, boostPadLayer))
        {
            velocityY = boostYAmt;
            transform.forward += transform.forward * boostForwardAmt;
        }
    }
}
