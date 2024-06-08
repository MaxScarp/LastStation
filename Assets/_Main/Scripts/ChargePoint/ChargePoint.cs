using System;
using TMPro;
using UnityEngine;

public class ChargePoint : MonoBehaviour, IInteractable, ITargatable
{
    private enum State
    {
        EMPTY,
        IDLE,
        CHARGING
    }

    public event EventHandler OnVehicleChargeCompleted;

    [SerializeField] private MeshRenderer visualMeshRenderer;

    [SerializeField] private Material chargingMaterial;
    [SerializeField] private Material idleMaterial;
    [SerializeField] private Material emptyMaterial;

    [SerializeField] private float chargeRateo = 0.1f;
    [SerializeField] private float chargeTimerMax = 3.0f;
    [SerializeField] private float chargeMax = 1.0f;

    [SerializeField] private Hint hintText;

    [SerializeField] private Transform target;

    [SerializeField] private Material hitMaterial;
    [SerializeField] private float maxHitTimer = 0.75f;

    [SerializeField] private Transform vehicleTarget;
    [SerializeField] private bool isDebug = false;

    [SerializeField] private Transform vehicleExitTarget;

    private State currentState;
    private float currentCharge;
    private float chargeTimer;

    private HealthSystem healthSystem;

    private bool isHit;
    private bool isHitMaterialSetted;
    private float hitTimer;

    private Material regularMaterial;

    public Vehicle Vehicle { get; set; }

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();

        if (isDebug)
        {
            currentCharge = 0.0f;
        }
        else
        {
            currentCharge = chargeMax;
        }

        chargeTimer = chargeTimerMax;

        hitTimer = maxHitTimer;
    }

    private void Start()
    {
        GameManager.Instance.AddChargePoint(this);

        hintText.gameObject.SetActive(false);

        if (currentCharge > 0.0f)
        {
            ChangeState(State.IDLE);
        }
        else
        {
            ChangeState(State.EMPTY);
        }
    }

    private void Update()
    {
        HandleHit();

        if (currentCharge <= 0.0f)
        {
            ChangeState(State.EMPTY);
        }

        if (currentState == State.CHARGING)
        {
            if (Vehicle.GetCurrentCharge() >= Vehicle.GetChargeMax())
            {
                ChangeState(State.IDLE);

                chargeTimer = chargeTimerMax;

                OnVehicleChargeCompleted?.Invoke(this, EventArgs.Empty);
                return;
            }

            chargeTimer -= Time.deltaTime;
            if (chargeTimer <= 0.0f)
            {
                chargeTimer = chargeTimerMax;

                currentCharge -= chargeRateo;
                Vehicle.AddCharge(chargeRateo);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            hintText.gameObject.SetActive(true);

            if (currentState == State.IDLE)
            {
                hintText.GetComponent<TextMeshPro>().text = "e";
            }
            else if (currentState == State.EMPTY)
            {
                hintText.GetComponent<TextMeshPro>().text = "f";
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            hintText.gameObject.SetActive(false);
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

    private void SetHitMaterials()
    {
        regularMaterial = visualMeshRenderer.sharedMaterial;
        visualMeshRenderer.sharedMaterial = hitMaterial;
    }

    private void ResetMaterials()
    {
        visualMeshRenderer.sharedMaterial = regularMaterial;
    }

    private void ChangeState(State newState)
    {
        currentState = newState;
        chargeTimer = chargeTimerMax;

        switch (newState)
        {
            case State.IDLE:
                visualMeshRenderer.sharedMaterial = idleMaterial;
                break;
            case State.CHARGING:
                visualMeshRenderer.sharedMaterial = chargingMaterial;
                break;
            case State.EMPTY:
                visualMeshRenderer.sharedMaterial = emptyMaterial;
                break;
            default:
                Debug.LogError($"Error: trying to set a material for state: {newState}!");
                break;
        }
    }

    private void Die()
    {
        GameManager.Instance.RemoveChargePoint(this);

        Destroy(gameObject);
    }

    public void Fill()
    {
        if (currentState == State.EMPTY)
        {
            if (ResourceManager.Instance.TryUseEnergyCell())
            {
                currentCharge = chargeMax;
                hintText.GetComponent<TextMeshPro>().text = "e";

                ChangeState(State.IDLE);
            }
        }
    }

    public void Interact()
    {
        if (currentCharge <= 0.0f) return;

        if (currentState == State.CHARGING)
        {
            ChangeState(State.IDLE);
            return;
        }

        if (Vehicle)
        {
            ChangeState(State.CHARGING);
        }
    }

    public void TakeDamage(float damageAmount)
    {
        healthSystem.TakeDamage(damageAmount);

        if (healthSystem.GetCurrentHealth() <= 0.0f)
        {
            Die();
        }
    }

    public void Hit(Gun gun = null)
    {
        isHit = true;
    }

    public float GetHealth() => healthSystem.GetCurrentHealth();
    public Transform GetTarget() => target;
    public Transform GetVehicleTarget() => vehicleTarget;
    public Transform GetVehicleExitTarget() => vehicleExitTarget;
}
