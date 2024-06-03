using System;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private const string SPEED = "speed";
    private const string IS_ARMED = "isArmed";

    private Player player;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        player = GetComponentInParent<Player>();
    }

    private void Start()
    {
        player.OnAmmoTaken += Player_OnAmmoTaken;
        player.OnAmmoFinished += Player_OnAmmoFinished;
    }

    private void Update()
    {
        animator.SetFloat(SPEED, Mathf.Clamp01(Mathf.Abs(Mathf.Abs(player.GetPlayerMovement().GetVelocity().magnitude))));
    }

    private void Player_OnAmmoFinished(object sender, EventArgs e)
    {
        SetIsArmed(false);
    }

    private void Player_OnAmmoTaken(object sender, EventArgs e)
    {
        SetIsArmed(true);
    }

    private void SetIsArmed(bool isArmed)
    {
        animator.SetBool(IS_ARMED, isArmed);
    }

}