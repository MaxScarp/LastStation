using System;
using UnityEngine;

public class VehicleSpawner : MonoBehaviour
{
    [SerializeField] private Transform VehiclePrefab;
    [SerializeField] private ChargePoint chargePoint;

    private float timerMax;
    private float timer;
    private bool isTimerStarted;

    private Vehicle spawnedVehicle;

    private void Awake()
    {
        timerMax = UnityEngine.Random.Range(0.0f, 3.0f);
        timer = timerMax;
    }

    private void Start()
    {
        GameManager.Instance.OnGameStarted += GameManger_OnGameStarted;
    }

    private void GameManger_OnGameStarted(object sender, EventArgs e)
    {
        isTimerStarted = true;
    }

    private void Update()
    {
        if (isTimerStarted)
        {
            timer -= Time.deltaTime;
            if (timer <= 0.0f)
            {
                timer = timerMax;
                isTimerStarted = false;

                SpawnVehicle();
            }
        }
    }

    private void SpawnVehicle()
    {
        Vehicle vehicle = Instantiate(VehiclePrefab, transform).GetComponent<Vehicle>();
        vehicle.Setup(chargePoint, chargePoint.GetVehicleExitTarget());

        vehicle.OnChargeCompleted += Vehicle_OnChargeCompleted;

        spawnedVehicle = vehicle;
    }

    private void Vehicle_OnChargeCompleted(object sender, EventArgs e)
    {
        spawnedVehicle.OnChargeCompleted -= Vehicle_OnChargeCompleted;

        isTimerStarted = true;
    }
}
