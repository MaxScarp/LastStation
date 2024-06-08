using System;
using UnityEngine;

public class Runner : Enemy
{
    public event EventHandler OnHeavyAttack;

    [SerializeField] private float heavyAttackDamage = 0.3f;

    protected override void TrySetTarget()
    {
        float minDistance = float.MaxValue;
        foreach (ChargePoint chargePoint in GameManager.Instance.GetChargePointList())
        {
            if (chargePoint.GetHealth() <= 0.0f) continue;

            float distance = Vector3.Distance(selfTarget.position, chargePoint.GetTarget().position);
            if (distance < minDistance)
            {
                minDistance = distance;
                target = chargePoint.GetTarget();
            }
        }

        Transform playerTargetTransform = GameManager.Instance.GetPlayer().GetTarget();
        float distanceFromPlayer = Vector3.Distance(selfTarget.position, playerTargetTransform.position);
        if (distanceFromPlayer <= minDistance)
        {
            target = playerTargetTransform;
        }
    }

    protected override void ChaseTarget()
    {
        base.ChaseTarget();

        if (target)
        {
            if (!target.GetComponent<Player>())
            {
                Transform playerTargetTransform = GameManager.Instance.GetPlayer().GetTarget();
                float distanceFromPlayer = Vector3.Distance(selfTarget.position, playerTargetTransform.position);
                float distanceFromTarget = Vector3.Distance(selfTarget.position, target.position);
                if (distanceFromPlayer <= distanceFromTarget)
                {
                    target = playerTargetTransform;
                }
            }
        }

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            ChangeState(State.DELAY);

            OnHeavyAttack?.Invoke(this, EventArgs.Empty);
        }
    }

    public override void Attack()
    {
        if (target)
        {
            if (target.parent.TryGetComponent(out Player player))
            {
                player.TakeDamage(damage);
                player.Hit();
            }
            else
            {
                ChargePoint chargePoint = target.GetComponentInParent<ChargePoint>();
                chargePoint.TakeDamage(damage);
                chargePoint.Hit();
            }
        }
    }

    public void HeavyAttack()
    {
        if (target)
        {
            if (target.parent.TryGetComponent(out Player player))
            {
                player.TakeDamage(heavyAttackDamage);
                player.Hit();
            }
            else
            {
                ChargePoint chargePoint = target.GetComponentInParent<ChargePoint>();
                chargePoint.TakeDamage(heavyAttackDamage);
                chargePoint.Hit();
            }
        }
    }

    protected override void Delay()
    {
        base.Delay();

        if (delayTimer < 0.0f)
        {
            ChangeState(State.ATTACK);
        }
    }

    protected override void SetHitMaterials()
    {
        visualMeshRenderer.sharedMaterial = hitMaterial0;
    }

    protected override void ResetMaterials()
    {
        visualMeshRenderer.sharedMaterial = regularMaterial0;
    }
}
