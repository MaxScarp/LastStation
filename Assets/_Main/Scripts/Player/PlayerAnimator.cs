using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private const string SPEED = "speed";

    [SerializeField] private PlayerMovement playerMovement;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetFloat(SPEED, Mathf.Clamp01(Mathf.Abs(Mathf.Abs(playerMovement.GetVelocity().magnitude))));
    }
}