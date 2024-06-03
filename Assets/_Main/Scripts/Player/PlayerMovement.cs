using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("MOVEMENT")]
    [SerializeField] private float smoothTime = 1.2f;
    [SerializeField] private float moveSpeed = 2.0f;

    private CharacterController characterController;
    private Vector2 currentInputVector;
    private Vector2 smoothInputVelocity;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        HandleRotation();
        HandleMovement();
    }

    private void HandleMovement()
    {
        currentInputVector = Vector2.SmoothDamp(currentInputVector, InputManager.Instance.GetMovementVector(), ref smoothInputVelocity, smoothTime);
        Vector3 moveDirX = currentInputVector.x * Vector3.right;
        Vector3 moveDirY = currentInputVector.y * Vector3.forward;
        Vector3 moveDir = moveDirX + moveDirY;

        characterController.Move(moveDir * moveSpeed * Time.deltaTime);
    }

    private void HandleRotation()
    {
        transform.LookAt(InputManager.Instance.GetMousePosition(), Vector3.up);
    }

    public Vector3 GetVelocity()
    {
        return characterController.velocity;
    }
}