using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BuoyancyObject : MonoBehaviour
{
    public float underWaterDrag = 3f;
    public float underWaterAngularDrag = 1f;
    public float airDrag = 0f;
    public float airAngularDrag = 0.05f;
    public float floatingPower = 130f;
    public float waterHeight = 0f;

    private Rigidbody mRigidbody;
    private bool underwater;
    
    
    void Start()
    {
        mRigidbody = GetComponent<Rigidbody>();
    }
    
    void FixedUpdate()
    {
        float difference = transform.position.y - waterHeight;

        if (difference < 0)
        {
            mRigidbody.AddForceAtPosition(Vector3.up * floatingPower * Mathf.Abs(difference), transform.position, ForceMode.Force);
            if (!underwater)
            {
                underwater = true;
                SwitchState(true);
            }
            
        }
        else if (underwater)
        {
            underwater = false;
        }
    }

    void SwitchState(bool isUnderwater)
    {
        if (isUnderwater)
        {
            mRigidbody.drag = underWaterDrag;
            mRigidbody.angularDrag = underWaterAngularDrag;
        }
        else
        {
            mRigidbody.drag = airDrag;
            mRigidbody.angularDrag = airAngularDrag;
        }
    }
}
