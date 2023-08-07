using UnityEngine;
using UnityEngine.UI;

public class CrosshairManager : MonoBehaviour
{
    public static bool isLookingAtShipWheel;
    public static bool isLookingAtEnemy;
    public static bool isLookingAtEnemyLong;
    public static bool isLookingAtHookTarget;
    public static bool isLookingAtChest;
    public static bool isLookingAtMushroom;
    public static RaycastHit crosshairHit;
    
    [Header("Assign")]
    [SerializeField] private float range = 7f;
    [SerializeField] private float longRange = 100f;
    [SerializeField] [Range(0, 1)] private float opacity = 0.3f;

    private Image crosshairImage;
    private Camera cam;
    private PlayerStateData psd;
    
    private Ray crosshairRay;
    private Color temporaryColor;

    [Header("Info - No Touch")]
    [SerializeField] private bool crosshairHighlightCondition;
    [SerializeField] private bool crosshairAttackCondition;
    
    [Header("Debug")]
    [SerializeField] private bool wheel;
    [SerializeField] private bool enemy;
    [SerializeField] private bool enemyLong;
    [SerializeField] private bool target;
    [SerializeField] private bool chest;
    [SerializeField] private bool mushroom;
    [SerializeField] private string lookName;

    private void Awake()
    {
        crosshairImage = GetComponentInChildren<Image>();
        cam = Camera.main;
        psd = GetComponentInParent<PlayerStateData>();
    }

    private void Update()
    {
        CastRay();
        CastLongRay();
        CalculateCrosshairHighlightCondition();

        //Just like the mesh renderer example, we can not directly change crosshairImage.color.a
        //We can only assign a color variable to it. Therefore we need a temporary color variable..
        //..to make changes upon and finally assign it
        
        temporaryColor = Color.white;
        if (crosshairAttackCondition)
        {
            temporaryColor = Color.red;
            temporaryColor.a = 1f;
        }
        else if (crosshairHighlightCondition) temporaryColor.a = 1f;
        else temporaryColor.a = opacity;
        crosshairImage.color = temporaryColor;

        HandleDebugInfo();
    }
    
    private void CastRay()
    {
        crosshairRay = cam.ScreenPointToRay(crosshairImage.rectTransform.position);

        if (!Physics.Raycast(crosshairRay, out crosshairHit, range))
        {
            isLookingAtShipWheel = false;
            isLookingAtEnemy = false;
            isLookingAtHookTarget = false;
            isLookingAtChest = false;
            return;
        }

        isLookingAtShipWheel = crosshairHit.collider.CompareTag("ShipWheel");
        isLookingAtEnemy = crosshairHit.collider.CompareTag("Enemy");
        isLookingAtHookTarget = crosshairHit.collider.CompareTag("HookPlace");
        isLookingAtChest = crosshairHit.collider.CompareTag("Chest");
        isLookingAtMushroom = crosshairHit.collider.CompareTag("Mushroom");
    }
    
    private void CastLongRay()
    {
        crosshairRay = cam.ScreenPointToRay(crosshairImage.rectTransform.position);

        if (!Physics.Raycast(crosshairRay, out crosshairHit, longRange))
        {
            isLookingAtHookTarget = false;
            return;
        }

        isLookingAtEnemyLong = crosshairHit.collider.CompareTag("Enemy");
        isLookingAtHookTarget = crosshairHit.collider.CompareTag("HookPlace") || isLookingAtEnemyLong;
    }

    private void CalculateCrosshairHighlightCondition()
    {
        if (psd.currentMainState is not (PlayerStateData.PlayerMainState.NormalState or PlayerStateData.PlayerMainState.HookState)) return;
        
        crosshairHighlightCondition = isLookingAtShipWheel || isLookingAtHookTarget || isLookingAtChest || isLookingAtMushroom;
        crosshairAttackCondition = isLookingAtEnemy;
    }

    private void HandleDebugInfo()
    {
        wheel = isLookingAtShipWheel;
        enemy = isLookingAtEnemy;
        enemyLong = isLookingAtEnemyLong;
        target = isLookingAtHookTarget;
        chest = isLookingAtChest;
        mushroom = isLookingAtMushroom;
        if (crosshairHit.collider != null) lookName = crosshairHit.collider.name;
    }
}
