using UnityEngine;

public class ExtraGravity : MonoBehaviour
{
    [SerializeField] private float extraGravity = 10f;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rb.AddForce(new Vector3(0f, - 1 * extraGravity, 0f), ForceMode.Acceleration);
    }
}