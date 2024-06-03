using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public event EventHandler OnInteractPerformed;
    public event EventHandler OnShootPerformed;
    public event EventHandler OnFillPerformed;

    [SerializeField] private LayerMask groundMask;

    private PlayerInputAsset playerInputAsset;

    private void Awake()
    {
        if (Instance)
        {
            Debug.LogError($"Error: There is more than one {transform} in the scene!");
            Destroy(gameObject);
            return;
        }

        Instance = this;

        playerInputAsset = new PlayerInputAsset();

        playerInputAsset.Player.Enable();

        playerInputAsset.Player.Interact.performed += Interact_performed;
        playerInputAsset.Player.Shoot.performed += Shoot_performed;
        playerInputAsset.Player.Fill.performed += Fill_performed;
    }

    private void Fill_performed(InputAction.CallbackContext obj)
    {
        OnFillPerformed?.Invoke(this, EventArgs.Empty);
    }

    private void Shoot_performed(InputAction.CallbackContext obj)
    {
        OnShootPerformed?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(InputAction.CallbackContext obj)
    {
        OnInteractPerformed?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVector() => playerInputAsset.Player.Movement.ReadValue<Vector2>();

    public Vector3 GetMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.value);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, groundMask))
        {
            return hitInfo.point;
        }

        return Vector3.zero;
    }
}
