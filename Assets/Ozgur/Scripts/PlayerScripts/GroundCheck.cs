using System;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public static event Action OnSwimmingEnter;
    public static event Action OnSwimmingExit;
    
    
    [Header("Assign")]
    [SerializeField] private float radius = 1f;
    [SerializeField] private float offset = 0.6f;

    [Header("Select")] [SerializeField] private bool gizmos;
    
    [Header("Info - No Touch")]
    [SerializeField] private Collider[] colliders;
    [SerializeField] private int collidedObjectNumber;

    private PlayerStateData psd;
    
    private bool previousIsSwimming;

    private void Awake()
    {
        psd = PlayerStateData.Singleton;
    }

    private void Update()
    {
        colliders = Physics.OverlapSphere(new Vector3(transform.position.x, transform.position.y - offset, transform.position.z), radius);
        collidedObjectNumber = colliders.Length;

        psd.isSwimming = false;
        foreach (Collider col in colliders)
        {
            if (col.isTrigger) collidedObjectNumber--;
            if (col.CompareTag("SeaGround")) psd.isSwimming = true;
        }
        
        psd.isGrounded = collidedObjectNumber > 1;
        if (psd.isSwimming) psd.isGrounded = false;

        CheckEnterExitSwimming();
    }

    private void CheckEnterExitSwimming()
    {
        if (previousIsSwimming && !psd.isSwimming) OnSwimmingExit?.Invoke();
        else if (!previousIsSwimming && psd.isSwimming) OnSwimmingEnter?.Invoke();

        previousIsSwimming = psd.isSwimming;
    }
    
    private void OnDrawGizmos()
    {
        if (!gizmos) return;
        
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y - offset, transform.position.z), radius);
    }
}
