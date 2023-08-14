using System.Collections;
using DG.Tweening;
using UnityEngine;

public class ShipWaterPhysics : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private float movingAmount = 1f;
    [SerializeField] private float movingTime = 1f;
    [SerializeField] private float rotatingAmount = 1f;
    [SerializeField] private float rotatingSpeed = 1f;

    [Header("Info - No Touch")]
    [SerializeField] private bool isXPositive;
    [SerializeField] private bool isZPositive;
    
    private bool isMoving;
    private bool isXRotating;
    private bool isZRotating;
    
    private IEnumerator movingAnimation;
    private IEnumerator xRotatingAnimation;
    private IEnumerator zRotatingAnimation;

    private void Awake()
    {
        movingAnimation = Move();
    }

    private void Update()
    {
        if (!isMoving)
        {
            movingAnimation = Move();
            StartCoroutine(movingAnimation);
        }

        if (!isXRotating)
        {
            if (isXPositive) xRotatingAnimation = Rotate(false, Axis.X);
            else xRotatingAnimation = Rotate(true, Axis.X);

            StartCoroutine(xRotatingAnimation);
        }
        
        if (!isZRotating)
        {
            if (isZPositive) zRotatingAnimation = Rotate(false, Axis.Z);
            else zRotatingAnimation = Rotate(true, Axis.Z);

            StartCoroutine(zRotatingAnimation);
        }
    }

    private IEnumerator Move()
    {
        isMoving = true;

        transform.DOMoveY(transform.position.y + movingAmount, movingTime).SetEase(Ease.InOutSine);
        yield return new WaitForSeconds(movingTime);

        transform.DOMoveY(transform.position.y - movingAmount, movingTime).SetEase(Ease.InOutSine);
        yield return new WaitForSeconds(movingTime);
        
        isMoving = false;
    }

    private enum Axis
    {
        X,Z
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

            Vector3 rotation = Vector3.zero;
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
