using System;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour, ITargatable
{
    protected enum State
    {
        NONE,
        IDLE,
        MOVE,
        ATTACK,
        DELAY,
        DIE
    }

    public event EventHandler OnAttack;
    public event EventHandler OnDie;

    [SerializeField] private float maxAttackTimer = 3.0f;

    [SerializeField] protected Material hitMaterial0;
    [SerializeField] protected Material hitMaterial1;
    [SerializeField] protected Material hitMaterial2;
    [SerializeField] protected Material regularMaterial0;
    [SerializeField] protected Material regularMaterial1;
    [SerializeField] protected Material regularMaterial2;
    [SerializeField] protected SkinnedMeshRenderer visualMeshRenderer;

    [SerializeField] protected float maxSearchTimer = 3.5f;
    [SerializeField] protected float maxDelayTimer = 5.0f;
    [SerializeField] protected float maxHitTimer = 0.75f;
    [SerializeField] protected float damage = 0.2f;

    [SerializeField] protected Transform selfTarget;

    private State currentState;

    private Vector3 currentTargetPosition;
    private float attackTimer;

    private bool isHit;
    private bool isHitMaterialSetted;
    private float hitTimer;

    private HealthSystem healthSystem;

    protected NavMeshAgent agent;
    protected float searchTimer;
    protected Transform target;

    protected float delayTimer;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        healthSystem = GetComponent<HealthSystem>();

        currentState = State.IDLE;

        hitTimer = maxHitTimer;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) //DEBUG
        {
            healthSystem.TakeDamage(1.5f);
        }

        HandleHit();

        if (healthSystem.GetCurrentHealth() <= 0.0f)
        {
            ChangeState(State.DIE);
            return;
        }

        if (target)
        {
            HandleRotation();
        }

        switch (currentState)
        {
            case State.IDLE:
                SearchForTarget();
                break;
            case State.MOVE:
                ChaseTarget();
                break;
            case State.ATTACK:
                WaitForAttack();
                break;
            case State.DELAY:
                Delay();
                break;
            default:
                Debug.LogError($"Error: Trying to managing state {currentState} into Update!");
                break;
        }
    }

    private void HandleHit()
    {
        if (isHit)
        {
            if (!isHitMaterialSetted)
            {
                isHitMaterialSetted = true;
                SetHitMaterials();
            }

            hitTimer -= Time.deltaTime;
            if (hitTimer <= 0.0f)
            {
                isHit = false;
                isHitMaterialSetted = false;
                hitTimer = maxHitTimer;

                ResetMaterials();
            }
        }
    }

    private void HandleRotation()
    {
        Vector3 lookDirection = (target.position - transform.position).normalized;
        lookDirection.y = 0.0f;
        transform.forward = lookDirection;
    }

    private void Die()
    {
        OnDie?.Invoke(this, EventArgs.Empty);
    }

    private void WaitForAttack()
    {
        if (!target)
        {
            ChangeState(State.IDLE);
            return;
        }

        Vector3 targetPosition = target.position;
        if (targetPosition != currentTargetPosition)
        {
            currentTargetPosition = targetPosition;
            ChangeState(State.MOVE);
            return;
        }

        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0.0f)
        {
            attackTimer = maxAttackTimer;

            OnAttack?.Invoke(this, EventArgs.Empty);
        }
    }

    protected virtual void Delay()
    {
        delayTimer -= Time.deltaTime;
    }

    protected virtual void ChaseTarget()
    {
        if (!target)
        {
            ChangeState(State.IDLE);
            return;
        }

        Vector3 targetPosition = target.position;
        if (targetPosition != currentTargetPosition)
        {
            currentTargetPosition = targetPosition;
            agent.SetDestination(target.position);
        }
    }

    protected void ChangeState(State newState)
    {
        if (currentState == newState) return;

        currentState = newState;

        switch (newState)
        {
            case State.IDLE:
                agent.isStopped = false;
                searchTimer = maxSearchTimer;
                break;
            case State.MOVE:
                agent.isStopped = false;
                break;
            case State.ATTACK:
                attackTimer = maxAttackTimer;
                break;
            case State.DIE:
                agent.isStopped = true;
                GetComponent<CharacterController>().enabled = false;
                Die();
                break;
            case State.DELAY:
                agent.isStopped = true;
                delayTimer = maxDelayTimer;
                break;
            default:
                Debug.LogError($"Error: Trying to managing state {currentState}!");
                break;
        }
    }

    private void SearchForTarget()
    {
        searchTimer -= Time.deltaTime;
        if (searchTimer <= 0.0f)
        {
            searchTimer = maxSearchTimer;

            TrySetTarget();

            if (!target) return;

            ChangeState(State.MOVE);
        }
    }

    protected abstract void TrySetTarget();

    protected abstract void SetHitMaterials();
    protected abstract void ResetMaterials();

    public abstract void Attack();

    public void Destroy()
    {
        Destroy(gameObject);
    }

    public void Hit(Gun gun)
    {
        isHit = true;

        if (gun)
        {
            healthSystem.TakeDamage(gun.GetDamage());
        }
    }

    public float GetVelocityVectorMagnitude() => agent.velocity.magnitude;
    public Transform GetTarget() => selfTarget;
}
