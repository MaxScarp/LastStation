public class GruntAnimator : EnemyAnimator
{
    private const string ATTACK = "attack";

    protected override void TriggerAttack()
    {
        animator.SetTrigger(ATTACK);
    }

    public void AttackEvent()
    {
        enemy.Attack();
    }
}
