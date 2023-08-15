using UnityEngine;

public class WaterShaderGizmo : MonoBehaviour
{
    private Collider col;
    private bool isColAssigned;

    private void OnDrawGizmosSelected()
    {
        if (!isColAssigned)
        {
            col = GetComponent<Collider>();
            isColAssigned = true;
        }
        
        Gizmos.color = Color.cyan;
        Gizmos.DrawCube(col.bounds.center, col.bounds.size);
    }
}
