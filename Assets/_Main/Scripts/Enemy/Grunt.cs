using System.Collections.Generic;
using UnityEngine;

public class Grunt : Enemy
{
    public override void Attack()
    {
        if (target)
        {
            ChargePoint chargePoint = target.GetComponentInParent<ChargePoint>();
            chargePoint.TakeDamage(damage);
            chargePoint.Hit();
        }
    }

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
    }

    protected override void ChaseTarget()
    {
        base.ChaseTarget();

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            ChangeState(State.ATTACK);
        }
    }

    protected override void Delay() { }

    protected override void SetHitMaterials()
    {
        visualMeshRenderer.SetSharedMaterials(new List<Material>() { hitMaterial0, hitMaterial1, hitMaterial2 });
    }

    protected override void ResetMaterials()
    {
        visualMeshRenderer.SetSharedMaterials(new List<Material>() { regularMaterial0, regularMaterial1, regularMaterial2 });
    }
}
