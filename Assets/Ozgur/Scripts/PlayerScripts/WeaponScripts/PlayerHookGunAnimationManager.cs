using UnityEngine;
using UnityEngine.UI;

public class PlayerHookGunAnimationManager : WeaponAnimationManagerBase
{
    [Header("Assign")]
    [SerializeField] private AudioSource hidingSource;
    
    private void Awake()
    {
        OnAwake();

        weaponTransform = GameObject.Find("PlayerCamera/HookGun").transform;
        cooldownSlider = GameObject.Find("PlayerCanvas/HookGunCooldownSlider").GetComponent<Slider>();
        baseHidingSource = hidingSource;
    }

    private void Update()
    {
        OnUpdate();
        HandleAttack();
    }
    
    private void HandleAttack()
    {
        if (PlayerInputManager.Singleton.isHookKeyDown && !isHidden) StartCoroutine(PlayAttackAnimation());
    }
}
