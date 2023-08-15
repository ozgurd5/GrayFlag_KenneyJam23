using System.Collections;
using UnityEngine;

public class PlayerWaterPhysics : FakeWaterPhysicsBase
{
    [Header("Assign")]
    [SerializeField] private float swimPosition = 2f;
    
    private Rigidbody playerRb;
    private ExtraGravity playerEg;
    
    private bool canPlayerSwimAnimationPlay;
    private bool isPlayerSinking;
    
    private void Awake()
    {
        playerRb = GetComponent<Rigidbody>();
        playerEg = GetComponent<ExtraGravity>();

        PlayerSwimmingManager.OnSwimmingEnter += EnableSwimmingAnimation;
        PlayerSwimmingManager.OnSwimmingExit += DisableSwimmingAnimation;
    }
    
    private void Update()
    {
        if (canPlayerSwimAnimationPlay) PlaySwimmingAnimations();
    }

    private void EnableSwimmingAnimation()
    {
        StartCoroutine(SinkPlayer());
    }

    private IEnumerator SinkPlayer()
    {
        isPlayerSinking = true;
        
        while (isPlayerSinking)
        {
            if (transform.position.y <= swimPosition)
            {
                playerRb.velocity = new Vector3(playerRb.velocity.x, 0f, playerRb.velocity.z);
                playerEg.enabled = false;
                playerRb.useGravity = false;

                isPlayerSinking = false;
                canPlayerSwimAnimationPlay = true;
            }

            else
            {
                yield return null;
            }
        }
    }
    
    private void DisableSwimmingAnimation()
    {
        canPlayerSwimAnimationPlay = false;
        
        playerEg.enabled = true;
        playerRb.useGravity = true;
    }
    
    private void OnDestroy()
    {
        PlayerSwimmingManager.OnSwimmingEnter -= EnableSwimmingAnimation;
        PlayerSwimmingManager.OnSwimmingExit -= DisableSwimmingAnimation;
    }
}
