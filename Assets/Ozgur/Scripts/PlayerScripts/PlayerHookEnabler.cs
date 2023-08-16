using UnityEngine;

public class PlayerHookEnabler : MonoBehaviour
{
    [SerializeField] private MeshRenderer hookGunMeshRenderer;
    private PlayerHookController hookController;
    private PlayerHookGunAnimationManager hookGunAnimationManager;
    
    private void Awake()
    {
        hookGunMeshRenderer = GameObject.Find("PlayerCamera/HookGun").GetComponent<MeshRenderer>();
        hookController = GetComponent<PlayerHookController>();
        hookGunAnimationManager = GetComponent<PlayerHookGunAnimationManager>();

        PlayerPowerUps.OnHookGunBought += EnableHookGun;
    }

    //TODO: remove before build
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H)) EnableHookGun();
    }

    private void EnableHookGun()
    {
        hookGunMeshRenderer.enabled = true;
        hookController.enabled = true;
        hookGunAnimationManager.enabled = true;
    }

    private void OnDestroy()
    {
        PlayerPowerUps.OnHookGunBought -= EnableHookGun;
    }
}
