using System;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    public event Action OnPlayerEnter;
    public event Action OnPlayerExit;
    
    private void OnTriggerEnter(Collider col)
    {
        if (!col.CompareTag("Player")) return;
        OnPlayerEnter?.Invoke();
    }

    private void OnTriggerExit(Collider col)
    {
        if (!col.CompareTag("Player")) return;
        OnPlayerExit?.Invoke();
    }
}
