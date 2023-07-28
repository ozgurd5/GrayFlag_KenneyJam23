using UnityEngine;
using UnityEngine.UI;

public class CrosshairManager : MonoBehaviour
{
    public static bool isLookingAtShipWheel;
    public static bool isLookingAtEnemy;
    public static bool isLookingAtHookTarget;
    public static RaycastHit crosshairHit;
    
    [Header("Assign")]
    [SerializeField] private float range = 7f;
    [SerializeField] private float longRange = 100f;
    [SerializeField] [Range(0, 1)] private float opacity = 0.3f;

    private Image crosshairImage;
    private Camera cam;
    
    private Ray crosshairRay;
    private Color temporaryColor;

    public bool crosshairHighlightCondition;

    public bool wheel;
    public bool enemy;
    public bool target;

    private void Awake()
    {
        crosshairImage = GetComponentInChildren<Image>();
        cam = Camera.main;
    }

    private void Update()
    {
        wheel = isLookingAtShipWheel;
        enemy = isLookingAtEnemy;
        target = isLookingAtHookTarget;
        
        CastRay();
        CastLongRay();
        CalculateCrosshairHighlightCondition();

        //Just like the mesh renderer example, we can not directly change crosshairImage.color.a
        //We can only assign a color variable to it. Therefore we need a temporary color variable..
        //..to make changes upon and finally assign it
        
        temporaryColor = crosshairImage.color;
        if (crosshairHighlightCondition) temporaryColor.a = 1f;
        else temporaryColor.a = opacity;
        crosshairImage.color = temporaryColor;
    }
    
    private void CastRay()
    {
        crosshairRay = cam.ScreenPointToRay(crosshairImage.rectTransform.position);

        if (!Physics.Raycast(crosshairRay, out crosshairHit, range))
        {
            isLookingAtShipWheel = false;
            isLookingAtEnemy = false;
            isLookingAtHookTarget = false;
            return;
        }

        isLookingAtShipWheel = crosshairHit.collider.CompareTag("ShipWheel");
        isLookingAtEnemy = crosshairHit.collider.CompareTag("Enemy");
        isLookingAtHookTarget = crosshairHit.collider.CompareTag("HookPlace");
    }
    
    private void CastLongRay()
    {
        crosshairRay = cam.ScreenPointToRay(crosshairImage.rectTransform.position);

        if (!Physics.Raycast(crosshairRay, out crosshairHit, longRange))
        {
            isLookingAtHookTarget = false;
            return;
        }
        
        isLookingAtHookTarget = crosshairHit.collider.CompareTag("HookPlace");
    }

    private void CalculateCrosshairHighlightCondition()
    {
        crosshairHighlightCondition = isLookingAtShipWheel || isLookingAtEnemy || isLookingAtHookTarget;
    }
}
