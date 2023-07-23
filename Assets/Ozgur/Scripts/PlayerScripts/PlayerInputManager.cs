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
    public bool isJumpKeyDown;
    public bool isAttackKeyDown;
    public bool isHookKeyDown;
    public bool isHookKeyUp;

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
        isJumpKeyDown = pia.Player.Jump.WasPressedThisFrame();
        isAttackKeyDown = pia.Player.Attack.WasPressedThisFrame();
        isHookKeyDown = pia.Player.Hook.WasPressedThisFrame();
        isHookKeyUp = pia.Player.Hook.WasReleasedThisFrame();
    }
}