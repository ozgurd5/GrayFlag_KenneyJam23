using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    private PlayerInputActions pia;

    public Vector2 moveInput;
    public bool isRunKey;

    private void Awake()
    {
        pia = new PlayerInputActions();
        pia.Player.Enable();
    }
    
    void Update()
    {
        moveInput = pia.Player.Movement.ReadValue<Vector2>();
        isRunKey = pia.Player.Run.IsPressed();
    }
}