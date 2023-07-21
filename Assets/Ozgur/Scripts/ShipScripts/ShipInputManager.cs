using UnityEngine;

public class ShipInputManager : MonoBehaviour
{
    [Header("Sensitivity")]
    public float mouseSensitivity = 0.2f;
    
    private PlayerInputActions pia;

    public Vector2 lookInput;
    public Vector2 moveInput;

    private void Awake()
    {
        pia = new PlayerInputActions();
        pia.Ship.Enable();
    }
    
    void Update()
    {
        lookInput = pia.Ship.Look.ReadValue<Vector2>();
        lookInput.x *= mouseSensitivity;
        lookInput.y *= mouseSensitivity;
        
        moveInput = pia.Ship.Movement.ReadValue<Vector2>();
    }
}
