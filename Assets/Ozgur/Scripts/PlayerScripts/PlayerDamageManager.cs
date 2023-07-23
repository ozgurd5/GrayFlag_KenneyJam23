using UnityEngine;

public class PlayerDamageManager : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private int health = 20;
    [SerializeField] private int knockbackForce = 600;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void GetHit()
    {
        health -= 3;
        rb.AddForce(knockbackForce * transform.forward * -1, ForceMode.Acceleration);
        CheckForDeath();
    }
    
    private void CheckForDeath()
    {
        if (health < 0) transform.position = respawnPoint.position;
    }
}
