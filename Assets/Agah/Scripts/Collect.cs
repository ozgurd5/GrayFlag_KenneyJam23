using UnityEngine;

public class Collect : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (PlayerInputManager.Singleton.isInteractKeyDown && other.CompareTag("Collectable"))
        {
            //skor++
            Destroy(gameObject);
        }

    }
}
