using UnityEngine;
using UnityEngine.UI;

public class PlayerSwordController : WeaponAnimationManagerBase
{
    [Header("Assign")]
    [SerializeField] private AudioSource attackSource;
    [SerializeField] private AudioSource hidingSource;
    [SerializeField] private ParticleSystem whiteAttackParticle;
    [SerializeField] private ParticleSystem yellowAttackParticle;

    [Header("Assign - Damage")]
    [SerializeField] private int defaultDamage = 25;
    [SerializeField] private int powerUpDamage = 40;

    private int damage;
    private ParticleSystem attackParticle;

    private void Awake()
    {
        OnAwake();

        weaponTransform = GameObject.Find("PlayerCamera/Sword").transform;
        cooldownSlider = GameObject.Find("PlayerCanvas/SwordCooldownSlider").GetComponent<Slider>();
        baseHidingSource = hidingSource;

        attackParticle = whiteAttackParticle;
        PlayerColorEnabler.OnYellowColorEnabled += EnableYellowParticle;
        
        damage = defaultDamage;
        MarketManager.OnChickenBought += IncreaseDamage;
    }

    private void Update()
    {
        OnUpdate();
        HandleAttack();
    }

    private void HandleAttack()
    {
        if (!PlayerInputManager.Singleton.isAttackKeyDown || isAttackAnimationPlaying || isHidden) return;
        StartCoroutine(PlayAttackAnimation());
        attackSource.Play();

        if (CrosshairManager.isLookingAtEnemy)
        {
            CrosshairManager.crosshairHit.collider.GetComponent<EnemyManager>().GetHit(transform.forward, damage);
            attackParticle.Play();
        }
    }

    private void EnableYellowParticle()
    {
        attackParticle = yellowAttackParticle;
    }
    
    private void IncreaseDamage()
    {
        damage = powerUpDamage;
    }
    
    private void OnDestroy()
    {
        PlayerColorEnabler.OnYellowColorEnabled -= EnableYellowParticle;
        MarketManager.OnChickenBought -= IncreaseDamage;
    }
}