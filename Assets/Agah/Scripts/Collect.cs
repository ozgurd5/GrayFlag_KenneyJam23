using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : MonoBehaviour
{
    PlayerInputManager input;
    private void OnTriggerEnter(Collider other)
    {
        if (input.isInteractKeyDown && other.CompareTag("Collectable"))
        {
            //skor++
            Destroy(gameObject);
        }

    }
}
