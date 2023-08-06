using UnityEngine;

public class ShipParenter : MonoBehaviour
{
    private void OnTriggerStay(Collider col)
    {
       if (col.CompareTag("Player") && col.transform.parent == null) col.transform.parent = transform.parent;
    }
    
    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player")) col.transform.parent = null;
    }
}
