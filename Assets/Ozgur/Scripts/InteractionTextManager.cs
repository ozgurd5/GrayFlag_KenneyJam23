using UnityEngine;

public class InteractionTextManager : MonoBehaviour
{
    private static int lastID;

    [Header("Info - No Touch")]
    [SerializeField] private bool isOpen;
    public int id;

    private Canvas canvas;
    private ChestManager cm;
    private ColorAltarManager cam;

    private void Awake()
    {
        id = lastID;
        lastID++;

        canvas = GetComponent<Canvas>();
        if (transform.parent.CompareTag("Chest")) cm = transform.parent.GetComponent<ChestManager>();
        else if (transform.parent.CompareTag("ColorAltar")) cam = transform.parent.GetComponent<ColorAltarManager>();
    }

    public void OpenInteractionText()
    {
        if (isOpen) return;
        if (transform.parent.CompareTag("Chest") && cm.isChestOpened) return;
        if (transform.parent.CompareTag("ColorAltar") && cam.isActivated) return;
        
        isOpen = true;
        canvas.enabled = true;
    }

    public void CloseInteractionText()
    {
        if (!isOpen) return;
        
        isOpen = false;
        canvas.enabled = false;
    }
}
