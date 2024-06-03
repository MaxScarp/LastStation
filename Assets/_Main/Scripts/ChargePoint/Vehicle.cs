using System;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
    public event EventHandler<float> OnChargeChanged;
    public event EventHandler OnChargeCompleted;

    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float stoppingDistance = 0.5f;
    [SerializeField] private bool isDebug = false;
    [SerializeField] private float delayTimerMax = 1.0f;

    private ChargePoint chargePoint;
    private Vector3 targetPosition;
    private Transform vehicleExitTarget;
    private bool isMoving;
    private float chargeMax;
    private float currentCharge;
    private int movingCounter;
    private int movingCounterMax;
    private float delayTimer;
    private bool isWaiting;

    private void Awake()
    {
        if (isDebug)
        {
            chargeMax = 1.0f;
            currentCharge = 0.95f;
        }
        else
        {
            chargeMax = UnityEngine.Random.Range(1, 3);
            currentCharge = UnityEngine.Random.Range(0.0f, chargeMax);
        }

        movingCounterMax = 2;
        delayTimer = delayTimerMax;
    }

    private void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * speed);
            if (Vector3.Distance(new Vector3(transform.position.x, targetPosition.y, transform.position.z), targetPosition) <= stoppingDistance)
            {
                chargePoint.Vehicle = this;
                isMoving = false;

                movingCounter++;
                if (movingCounter >= movingCounterMax)
                {
                    Destroy(gameObject);
                }
            }

            HandleRoation();
        }

        if (isWaiting)
        {
            delayTimer -= Time.deltaTime;
            if (delayTimer <= 0.0f)
            {
                isWaiting = false;
                isMoving = true;
            }
        }
    }

    private void HandleRoation()
    {
        transform.forward = (targetPosition - transform.position).normalized;
    }

    public void Setup(ChargePoint chargePoint, Transform vehicleExitTarget)
    {
        this.chargePoint = chargePoint;
        this.vehicleExitTarget = vehicleExitTarget;

        Vector3 targetFixedPosition = new Vector3(chargePoint.GetVehicleTarget().position.x, transform.position.y, chargePoint.GetVehicleTarget().position.z);
        targetPosition = targetFixedPosition;

        isMoving = true;
    }

    public void AddCharge(float charge)
    {
        currentCharge += charge;

        OnChargeChanged?.Invoke(this, GetCurrentChargeNormalized());

        if (currentCharge >= chargeMax)
        {
            targetPosition = new Vector3(vehicleExitTarget.position.x, transform.position.y, vehicleExitTarget.position.z);
            isWaiting = true;

            OnChargeCompleted?.Invoke(this, EventArgs.Empty);
        }
    }

    public float GetCurrentCharge() => currentCharge;
    public float GetCurrentChargeNormalized() => currentCharge / chargeMax;
    public float GetChargeMax() => chargeMax;
}
