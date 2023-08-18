using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    public static int collidedObjectNumber { get; private set; }

    [Header("Assign")]
    [SerializeField] private float radius = 0.8f;//1f;
    [SerializeField] private float offset = 0.8f;//0.6f;

    [Header("Select")] [SerializeField] private bool gizmos;
    
    [Header("Info - No Touch")]
    [SerializeField] private Collider[] colliders;
    [SerializeField] private int collidedObjectNumberDebug;

    private PlayerStateData psd;

    private void Awake()
    {
        psd = PlayerStateData.Singleton;
    }

    private void Update()
    {
        colliders = Physics.OverlapSphere(new Vector3(transform.position.x, transform.position.y - offset, transform.position.z), radius);
        collidedObjectNumber = colliders.Length - 1; //must not include player

        psd.isSwimming = false;
        foreach (Collider col in colliders)
        {
            if (col.isTrigger) collidedObjectNumber--;
            if (col.CompareTag("Sea")) psd.isSwimming = true;
        }
        
        psd.isGrounded = collidedObjectNumber > 0;
        if (psd.isSwimming) psd.isGrounded = false;

        UpdateDebugValue();
    }

    private void UpdateDebugValue()
    {
        collidedObjectNumberDebug = collidedObjectNumber;
    }
    
    private void OnDrawGizmos()
    {
        if (!gizmos) return;
        
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y - offset, transform.position.z), radius);
    }
}