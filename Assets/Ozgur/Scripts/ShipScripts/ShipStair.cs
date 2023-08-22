using UnityEngine;

public class ShipStair : MonoBehaviour
{
    [Header("Assign")] [SerializeField] private float stairSpeed = 10f;
    
    private Rigidbody playerRb;
    
    private void Awake()
    {
        playerRb = GameObject.Find("Player").GetComponent<Rigidbody>();
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("Player")) playerRb.velocity = new Vector3(playerRb.velocity.x, stairSpeed, playerRb.velocity.z);
    }
}
