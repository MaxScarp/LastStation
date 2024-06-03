using System;

public class RunnerAnimator : EnemyAnimator
{
    private const string IS_LIGHT_ATTACK = "isLightAttack";
    private const string HEAVY_ATTACK = "heavyAttack";

    protected override void Start()
    {
        base.Start();

        ((Runner)enemy).OnHeavyAttack += Runner_OnHeavyAttack;
    }

    private void Runner_OnHeavyAttack(object sender, EventArgs e)
    {
        TriggerHeavyAttack();
    }

    protected override void TriggerAttack()
    {
        animator.SetTrigger(IS_LIGHT_ATTACK);
    }

    public void TriggerHeavyAttack()
    {
        animator.SetTrigger(HEAVY_ATTACK);
    }

    public void HeavyAttackEvent()
    {
        ((Runner)enemy).HeavyAttack();
    }

    public void AttackEvent()
    {
        enemy.Attack();
    }
}
