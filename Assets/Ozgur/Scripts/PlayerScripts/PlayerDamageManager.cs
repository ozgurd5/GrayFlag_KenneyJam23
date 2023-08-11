using UnityEngine;
using UnityEngine.UI;

public class PlayerDamageManager : MonoBehaviour
{
    [Header("Assign Manually")]
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private GameObject playerDamageEffect;
    
    [Header("Assign")]
    [SerializeField] private int health = 20;
    [SerializeField] private int knockbackForce = 1000;
    [SerializeField] private float damageStopTime = 0.5f;

    [Header("Assign - Sound")]
    [SerializeField] private AudioSource aus;
    [SerializeField] private AudioClip damageSound;
    [SerializeField] private AudioClip deathSound;
    
    private PlayerStateData psd;
    private Rigidbody rb;
    private Slider healthBar;

    private void Awake()
    {
        psd = PlayerStateData.Singleton;
        rb = GetComponent<Rigidbody>();
        healthBar = GetComponentInChildren<Slider>();
    }

    public void GetHit(Vector3 enemyTransformForward)
    {
        health -= 3;
        healthBar.value = health;
        
        rb.AddForce(20f * transform.up);
        rb.AddForce(knockbackForce * enemyTransformForward, ForceMode.Acceleration);
        
        psd.isGettingDamage = true;
        playerDamageEffect.SetActive(true);
        Invoke(nameof(SetIsGettingDamageFalse), damageStopTime);

        CheckForDeath();
    }

    private void SetIsGettingDamageFalse()
    {
        psd.isGettingDamage = false;
        playerDamageEffect.SetActive(false);
    }
    
    private void CheckForDeath()
    {
        if (health < 0)
        {
            transform.position = respawnPoint.position;
            health = 20;
            healthBar.value = health;
            
            aus.PlayOneShot(deathSound);
        }

        else aus.PlayOneShot(damageSound);
    }
}
