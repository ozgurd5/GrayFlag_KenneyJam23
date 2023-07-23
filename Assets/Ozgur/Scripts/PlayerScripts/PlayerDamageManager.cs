using UnityEngine;

public class PlayerDamageManager : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private int health = 20;
    [SerializeField] private int knockbackForce = 1000;
    [SerializeField] private float damageStopTime = 0.5f;

    private PlayerStateData psd;
    private Rigidbody rb;

    private void Awake()
    {
        psd = GetComponent<PlayerStateData>();
        rb = GetComponent<Rigidbody>();
    }

    public void GetHit(Vector3 enemyTransformForward)
    {
        health -= 3;
        rb.AddForce(20f * transform.up);
        rb.AddForce(knockbackForce * enemyTransformForward, ForceMode.Acceleration);
        psd.isGettingDamage = true;
        Invoke(nameof(SetIsGettingDamageFalse), damageStopTime);
        CheckForDeath();
    }

    private void SetIsGettingDamageFalse()
    {
        psd.isGettingDamage = false;
    }
    
    private void CheckForDeath()
    {
        if (health < 0)
        {
            transform.position = respawnPoint.position;
            health = 20;
        }
    }
}
