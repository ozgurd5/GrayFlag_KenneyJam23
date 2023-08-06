using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Floater : MonoBehaviour
{
    public Rigidbody rigidBody;
    public float depthBeforeSubmerged = 1f;
    public float displacementAmount = 3f;
    public int floaterCount = 4;
    public float waterDrag = 6f;
    public float waterAngularDrag = 15f;

    private void FixedUpdate()
    {
        rigidBody.AddForceAtPosition(Physics.gravity / floaterCount,transform.position, ForceMode.Acceleration);
        float waveHeight = WaveManager.instance.GetWaveHeight(transform.position.x) + WaveManager.instance.transform.position.y;
        if (transform.position.y < waveHeight)
        {
            float displacementMultiplier = Mathf.Clamp01((waveHeight-transform.position.y) / depthBeforeSubmerged) * displacementAmount;
            rigidBody.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y)* displacementMultiplier,0f), transform.position, ForceMode.Acceleration);
            rigidBody.AddForce(displacementMultiplier * -rigidBody.velocity * waterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
            rigidBody.AddTorque(displacementMultiplier * -rigidBody.angularVelocity * waterAngularDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
    }
}
