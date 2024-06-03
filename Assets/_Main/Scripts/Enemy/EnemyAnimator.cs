using System;
using UnityEngine;

public abstract class EnemyAnimator : MonoBehaviour
{
    private const string RUNNING = "isRunning";
    protected const string DIE = "die";

    protected Enemy enemy;
    protected Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemy = GetComponentInParent<Enemy>();
    }

    protected virtual void Start()
    {
        enemy.OnAttack += Enemy_OnAttack;
        enemy.OnDie += Enemy_OnDie;
    }

    private void Enemy_OnDie(object sender, EventArgs e)
    {
        TriggerDie();
    }

    private void Enemy_OnAttack(object sender, EventArgs e)
    {
        TriggerAttack();
    }

    private void Update()
    {
        animator.SetBool(RUNNING, enemy.GetVelocityVectorMagnitude() > 0.0f);
    }

    private void TriggerDie()
    {
        animator.SetTrigger(DIE);
    }

    protected abstract void TriggerAttack();

    public void DieEvent()
    {
        enemy.Destroy();
    }
}
