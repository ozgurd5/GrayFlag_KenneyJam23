using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDamageManager : MonoBehaviour
{
    [Header("Assign Manually")]
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private GameObject playerDamageEffect;
    
    [Header("Assign")]
    [SerializeField] private int defaultHealth = 20;
    [SerializeField] private int powerUpHealth = 30;
    [SerializeField] private int knockBackForce = 1500;
    [SerializeField] private float damageStopTime = 0.5f;

    [Header("Assign - Sound")]
    [SerializeField] private AudioSource aus;
    [SerializeField] private AudioClip damageSound;
    [SerializeField] private AudioClip deathSound;
    
    private PlayerStateData psd;
    private Rigidbody rb;
    private Slider healthBar;
    
    private int health;

    private void Awake()
    {
        psd = PlayerStateData.Singleton;
        rb = GetComponent<Rigidbody>();
        healthBar = GetComponentInChildren<Slider>();
        
        health = defaultHealth;
        PlayerPowerUps.OnFishBought += IncreaseHealth;
    }

    public void GetHit(Vector3 enemyTransformForward)
    {
        health -= 3;
        healthBar.value = health;
        
        rb.AddForce(20f * transform.up);
        rb.AddForce(knockBackForce * enemyTransformForward, ForceMode.Acceleration);
        
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
            health = defaultHealth;
            healthBar.value = health;
            
            aus.PlayOneShot(deathSound);
        }
        
        else aus.PlayOneShot(damageSound);
    }

    private void IncreaseHealth()
    {
        defaultHealth = powerUpHealth;
        health = powerUpHealth;
        healthBar.maxValue = powerUpHealth;
        
        healthBar.value = powerUpHealth;
    }

    private void OnDestroy()
    {
        PlayerPowerUps.OnFishBought -= IncreaseHealth;
    }
}
