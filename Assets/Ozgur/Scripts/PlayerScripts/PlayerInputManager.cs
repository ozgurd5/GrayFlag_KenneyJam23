using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    [Header("Sensitivity")]
    public float mouseSensitivity = 0.2f;
    
    private PlayerInputActions pia;

    public Vector2 lookInput;
    public Vector2 moveInput;
    public bool isRunKey;
    public bool isInteractKeyDown;

    private void Awake()
    {
        pia = new PlayerInputActions();
        pia.Player.Enable();
    }
    
    void Update()
    {
        lookInput = pia.Player.Look.ReadValue<Vector2>();
        lookInput.x *= mouseSensitivity;
        lookInput.y *= mouseSensitivity;
        
        moveInput = pia.Player.Movement.ReadValue<Vector2>();
        isRunKey = pia.Player.Run.IsPressed();
        isInteractKeyDown = pia.Player.Interact.WasPressedThisFrame();
    }
}