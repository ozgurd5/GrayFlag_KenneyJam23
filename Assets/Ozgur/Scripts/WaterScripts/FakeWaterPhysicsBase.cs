using System;
using System.Collections;
using UnityEngine;

public class FakeWaterPhysicsBase : MonoBehaviour
{
    [Header("Assign")]
    private float movingAmount = 1f;
    private float movingSpeed = 1f;
    private float rotatingAmount = 1f;
    private float rotatingSpeed = 1f;
    
    private bool isXPositive;
    private bool isZPositive;
    private bool isMovingUp;
    
    private bool isMoving;
    private bool isXRotating;
    private bool isZRotating;

    protected enum Axis
    {
        X,Z
    }

    protected void PlaySwimmingAnimations()
    {
        if (!isMoving)
        {
            if (isMovingUp) StartCoroutine(Move(false));
            else StartCoroutine(Move(true));
        }
        
        if (!isXRotating)
        {
            if (isXPositive) StartCoroutine(Rotate(false, Axis.X));
            else StartCoroutine(Rotate(true, Axis.X));
        }
    
        if (!isZRotating)
        {
            if (isZPositive) StartCoroutine(Rotate(false, Axis.Z));
            else StartCoroutine(Rotate(true, Axis.Z));
        }
    }

    private IEnumerator Move(bool isUp)
    {
        isMoving = true;

        float totalMove = 0f;
        while (totalMove <= movingAmount)
        {
            float move = movingSpeed * Time.deltaTime;
            totalMove += move;

            if (totalMove > movingAmount) move -= totalMove - movingAmount;

            Vector3 nextPosition = transform.position;
            
            if (isUp) nextPosition.y += EaseInOutSine(totalMove) - EaseInOutSine(totalMove - move);
            else nextPosition.y -= EaseInOutSine(totalMove) - EaseInOutSine(totalMove - move);

            transform.position = nextPosition;

            yield return null;
        }

        isMoving = false;
        isMovingUp = isUp;
    }
    
    private float EaseInOutSine(float x)
    {
        return (float)-(Math.Cos(Math.PI * x) - 1) / 2;
    }
    
    private IEnumerator Rotate(bool isPositive, Axis axis)
    {
        if (axis == Axis.X) isXRotating = true;
        else isZRotating = true;
        
        float totalAngle = 0;
        while (totalAngle <= rotatingAmount)
        {
            float angle = rotatingSpeed * Time.deltaTime;
            totalAngle += angle;

            if (totalAngle > rotatingAmount) angle -= totalAngle - rotatingAmount;

            if (!isPositive) angle = -angle;

            Vector3 rotation;
            if (axis == Axis.X) rotation = new Vector3(angle, 0f, 0f);
            else rotation = new Vector3(0f, 0f, angle);
            
            transform.Rotate(rotation);

            yield return null;
        }

        if (axis == Axis.X) isXPositive = isPositive;
        else isZPositive = isPositive;

        if (axis == Axis.X) isXRotating = false;
        else isZRotating = false;
    }
}
