using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 offset;

    private Transform playerTransforn;

    private void Awake()
    {
        playerTransforn = GameObject.Find("Player").transform;
    }

    private void Update()
    {
        transform.position = playerTransforn.position + offset;
    }
}
