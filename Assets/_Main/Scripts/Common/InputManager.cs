using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    [SerializeField] private LayerMask groundMask;

    private PlayerInputAsset playerInputAsset;

    private Vector3 lastMousePosition;

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
