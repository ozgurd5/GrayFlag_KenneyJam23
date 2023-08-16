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
    public static bool isLookingAtColorAltar;
    
    public static RaycastHit crosshairHit;
    public static Transform enemyHookPlace;
    
    [Header("Assign")]
    [SerializeField] private float range = 10f;
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
    [SerializeField] private bool hookTarget;
    [SerializeField] private bool enemy;
    [SerializeField] private bool enemyLong;
    [SerializeField] private bool chest;
    [SerializeField] private bool mushroom;
    [SerializeField] private bool colorAltar;
    [SerializeField] private string lookName;

    private void Awake()
    {
        crosshairImage = GetComponentInChildren<Image>();
        cam = Camera.main;
        psd = PlayerStateData.Singleton;
    }

    private void Update()
    {
        CastRay();
        CastLongRay();
        GetEnemyHookPlace();
        
        CalculateCrosshairHighlightCondition();
        HighlightCrosshair();

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
            isLookingAtColorAltar = false;
            //ADD NEW HERE 1
            return;
        }

        isLookingAtShipWheel = crosshairHit.collider.CompareTag("ShipWheel");
        isLookingAtHookTarget = crosshairHit.collider.CompareTag("HookPlace");
        isLookingAtChest = crosshairHit.collider.CompareTag("Chest");
        isLookingAtMushroom = crosshairHit.collider.CompareTag("Mushroom");
        isLookingAtColorAltar = crosshairHit.collider.CompareTag("ColorAltar");
        //ADD NEW HERE 2
        
        isLookingAtEnemy = crosshairHit.collider.CompareTag("Enemy");
        if (isLookingAtEnemy) CheckForDeadEnemy();
    }
    
    private void CastLongRay()
    {
        crosshairRay = cam.ScreenPointToRay(crosshairImage.rectTransform.position);

        if (!Physics.Raycast(crosshairRay, out crosshairHit, longRange))
        {
            isLookingAtHookTarget = false;
            isLookingAtEnemyLong = false;
            return;
        }
        
        isLookingAtEnemyLong = crosshairHit.collider.CompareTag("Enemy");
        if (isLookingAtEnemyLong) CheckForDeadEnemy();
        
        isLookingAtHookTarget = crosshairHit.collider.CompareTag("HookPlace") || isLookingAtEnemyLong;
    }

    private void GetEnemyHookPlace()
    {
        if (isLookingAtEnemy || isLookingAtEnemyLong) enemyHookPlace = crosshairHit.transform.Find("EnemyHookPlace");
    }

    private void CalculateCrosshairHighlightCondition()
    {
        if (psd.currentMainState is not (PlayerStateData.PlayerMainState.NormalState or PlayerStateData.PlayerMainState.HookState)) return;
        
        crosshairHighlightCondition = isLookingAtShipWheel || isLookingAtHookTarget || isLookingAtChest || isLookingAtMushroom
            || isLookingAtColorAltar; //ADD NEW HERE 3
        
        crosshairAttackCondition = isLookingAtEnemy;
    }

    private void HighlightCrosshair()
    {
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
    }

    private void CheckForDeadEnemy()
    {
        if (crosshairHit.collider.GetComponent<EnemyManager>().currentState == EnemyManager.EnemyState.Dead)
        {
            isLookingAtEnemy = false;
            isLookingAtEnemyLong = false;
        }
    }

    private void HandleDebugInfo()
    {
        wheel = isLookingAtShipWheel;
        hookTarget = isLookingAtHookTarget;
        enemy = isLookingAtEnemy;
        enemyLong = isLookingAtEnemyLong;
        chest = isLookingAtChest;
        mushroom = isLookingAtMushroom;
        colorAltar = isLookingAtColorAltar;
        //ADD NEW HERE 4
        //ADD NEW HERE 5 IS IN PlayerInteractionManager.cs. Add it there if it is an interactable
        if (crosshairHit.collider != null) lookName = crosshairHit.collider.name;
    }
}