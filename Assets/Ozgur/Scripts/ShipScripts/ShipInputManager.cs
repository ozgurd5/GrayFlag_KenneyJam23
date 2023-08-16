using UnityEngine;

public class ShipInputManager : MonoBehaviour
{
    [Header("Info - No touch")]
    public Vector2 lookInput;
    public float rotateInput;
    public bool isSailUpKeyDown;
    public bool isSailDownKeyDown;

    private PlayerInputActions pia;

    private void Awake()
    {
        pia = new PlayerInputActions();
        pia.Ship.Enable();
    }
    
    void Update()
    {
        lookInput = pia.Ship.Look.ReadValue<Vector2>();
        lookInput.x *= PlayerInputManager.Singleton.mouseSensitivity;
        lookInput.y *= PlayerInputManager.Singleton.mouseSensitivity;

        isSailUpKeyDown = pia.Ship.SailUp.WasPressedThisFrame();
        isSailDownKeyDown = pia.Ship.SailDown.WasPressedThisFrame();

        rotateInput = pia.Ship.Rotation.ReadValue<float>();
    }
}
