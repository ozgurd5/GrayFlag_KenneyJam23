using UnityEngine;
using UnityEngine.UI;

public class CrosshairManager : MonoBehaviour
{
    public static bool isLookingAtInteractable;
    public static string interactableTag;
    public static RaycastHit crosshairHit;
    
    [Header("Assign")]
    [SerializeField] private float range = 7f;
    [SerializeField] [Range(0, 1)] private float opacity = 0.3f;

    private Image crosshairImage;
    private Camera cam;
    
    private Ray crosshairRay;
    private Color temporaryColor;

    private void Awake()
    {
        crosshairImage = GetComponentInChildren<Image>();
        cam = Camera.main;
    }

    private void Update()
    {
        CastRay();

        //Just like the mesh renderer example, we can not directly change crosshairImage.color.a
        //We can only assign a color variable to it. Therefore we need a temporary color variable..
        //..to make changes upon and finally assign it
        
        temporaryColor = crosshairImage.color;
        if (isLookingAtInteractable) temporaryColor.a = 1f;
        else temporaryColor.a = opacity;
        crosshairImage.color = temporaryColor;
    }
    
    private void CastRay()
    {
        crosshairRay = cam.ScreenPointToRay(crosshairImage.rectTransform.position);

        if (!Physics.Raycast(crosshairRay, out crosshairHit, range))
        {
            isLookingAtInteractable = false;
            return;
        }

        isLookingAtInteractable = crosshairHit.collider.CompareTag("ShipWheel");

        if (isLookingAtInteractable) interactableTag = crosshairHit.collider.tag;
    }
}
