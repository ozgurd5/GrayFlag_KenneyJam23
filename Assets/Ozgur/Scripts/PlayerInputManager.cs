using UnityEngine;

/// <summary>
/// <para>Gather and holds data from the unity input system</para>
/// <para>Works for only local player</para>
/// </summary>
public class PlayerInputManager : MonoBehaviour
{
    private PlayerInputActions pia;
    private Transform cameraTransform;

    public Vector2 moveInput;
    public Vector3 lookingDirectionForward;
    public bool isRunKey;

    private void Awake()
    {
        pia = new PlayerInputActions();
        pia.Player.Enable();
        cameraTransform = Camera.main!.transform;
    }

    private void Update()
    {
       moveInput = pia.Player.Movement.ReadValue<Vector2>();
       isRunKey = pia.Player.Run.IsPressed();
    }
    
    //Methods that depend lookingDirectionForward in PlayerController.cs are working in FixedUpdate, so we can calculate..
    //..and sync lookingDirectionForward in FixedUpdate
    private void FixedUpdate()
    {
        lookingDirectionForward = cameraTransform.forward;
        lookingDirectionForward.y = 0f;
    }
}