using System;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private int health = 10;
    [SerializeField] private int knockbackForce = 600;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void GetHit(Vector3 playerTransformForward)
    {
        health -= 3;
        
        rb.AddForce(knockbackForce * playerTransformForward, ForceMode.Force);
    }

    private void CheckForDeath()
    {
        if (health < 0) Destroy(gameObject);
    }
}