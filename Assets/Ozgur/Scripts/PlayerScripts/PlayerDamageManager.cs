using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDamageManager : MonoBehaviour
{
    public static event Action OnPlayerDeath;
    
    [Header("Assign Manually")]
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private GameObject playerDamageEffect;

    [Header("Assign")]
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private int defaultHealth = 100;
    [SerializeField] private int powerUpHealth = 150;
    [SerializeField] private int knockBackForce = 1000;
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
        MarketManager.OnFishBought += IncreaseHealth;
    }

    public void GetHit(Vector3 enemyTransformForward, int damage)
    {
        health -= damage;
        healthBar.value = health;
        healthText.text = $"{health}";
        
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
        if (health <= 0)
        {
            transform.position = respawnPoint.position;
            StartCoroutine(SetRbKinematicAndBack());
            aus.PlayOneShot(deathSound);
            
            health = defaultHealth;
            healthBar.value = health;
            healthText.text = $"{health}";
            
            OnPlayerDeath?.Invoke();
        }
        
        else aus.PlayOneShot(damageSound);
    }

    private IEnumerator SetRbKinematicAndBack()
    {
        rb.isKinematic = true;
        yield return null;
        rb.isKinematic = false;
    }

    private void IncreaseHealth()
    {
        defaultHealth = powerUpHealth;
        health = powerUpHealth;
        healthBar.maxValue = powerUpHealth;
        
        healthBar.value = powerUpHealth;
        healthText.text = $"{powerUpHealth}";
    }

    private void OnDestroy()
    {
        MarketManager.OnFishBought -= IncreaseHealth;
    }
}
