using UnityEngine;

public class ShipCollisionDetector : MonoBehaviour
{
    private ShipController sc;

    private void Awake()
    {
        sc = transform.parent.parent.GetComponent<ShipController>();
        
        sc.canMoveForward = true;
        sc.canMoveBackward = true;
        sc.canRotateRight = true;
        sc.canRotateLeft = true;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Sea") || col.CompareTag("Player")) return;
        
        if (name == "FrontShield")
        {
            Debug.Log("Enter: " + col.name);
            sc.StopCoroutine(sc.stopShipWithoutPhysicsCoroutine);
            sc.canMoveForward = false;
        }

        if (name == "BackShield") sc.canMoveBackward = false;
        if (name is "RightShieldFront" or "LeftShieldBack") sc.canRotateRight = false;
        if (name is "LeftShieldFront" or "RightShieldBack") sc.canRotateLeft = false;
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Sea") || col.CompareTag("Player")) return;

        if (name == "FrontShield")
        {
            Debug.Log("Exit: " + col.name);
            sc.canMoveForward = true;
        }
        if (name == "BackShield") sc.canMoveBackward = true;
        if (name is "RightShieldFront" or "LeftShieldBack") sc.canRotateRight = true;
        if (name is "LeftShieldFront" or "RightShieldBack") sc.canRotateLeft = true;
    }
}
