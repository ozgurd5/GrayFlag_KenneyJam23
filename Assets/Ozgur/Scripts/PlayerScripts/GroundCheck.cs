using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private PlayerStateData psd;

    private void Awake()
    {
        psd = GetComponent<PlayerStateData>();
    }

    private void Update()
    {
        psd.isGrounded = Physics.Raycast(transform.position, Vector3.down, 2f);
    }
}
