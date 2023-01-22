using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloaterEffectController : MonoBehaviour
{
    public Rigidbody floaterRigidbody;
    public float balancedUnderWaterHeight = 1f;     // distance from center point to bottom, when balanced
    public float buoyancyMultiplier = 3f;           // buoyancy when full underwater body is under water

    private void FixedUpdate()
    {
        if (transform.position.y < 0f)
        {
            var buoyancy = Mathf.Abs(Mathf.Clamp01(-transform.position.y / balancedUnderWaterHeight) * buoyancyMultiplier * Physics.gravity.y);
            floaterRigidbody.AddForce(new Vector3(0f, buoyancy, 0f), ForceMode.Acceleration);
        }
    }
}
