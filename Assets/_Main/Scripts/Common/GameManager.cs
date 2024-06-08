using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event EventHandler OnGameStarted;
    public event EventHandler<OnWaveSpawnedEventArgs> OnWaveSpawned;
    public class OnWaveSpawnedEventArgs : EventArgs
    {
        public int WaveCounter;
        public int WaveLevel;
    }
    public event EventHandler OnGameEnded;
    public event EventHandler<float> OnGameTimeChanged;
    public event EventHandler<float> OnStartingTimeChanged;
    public event EventHandler<float> OnWaveTimeChanged;

    [SerializeField] private float timerMax = 5.0f;

    [SerializeField] private Transform energyCellPrefab;
    [SerializeField] private Transform ammoPrefab;
    [SerializeField] private EnergyCell[] energyCellArray;
    [SerializeField] private Ammo[] ammoArray;
    [SerializeField] private Transform energyCellContainer;
    [SerializeField] private Transform ammoContainer;

    [SerializeField] private AudioSource environmentMusic;

    private List<ChargePoint> chargePointList;
    private float timer;
    private bool isTimerStarted;
    private float gameTime;
    private bool isGameStarted;
    private float waveTimer;
    private float waveTimerMax;
    private int waveCounter;
    private int waveCounterMax;
    private int waveLevel;
    private Vector3[] energyCellPostionArray;
    private Vector3[] ammoPostionArray;

    public Player Player { get; set; }

    private void Awake()
    {
        if (Instance)
        {
            Debug.LogError($"Error: There is more than one {transform} in the scene!");
            Destroy(gameObject);
            return;
        }

        Instance = this;

        chargePointList = new List<ChargePoint>();

        timer = timerMax;
        gameTime = 0.0f;
        waveLevel = 1;
        waveCounterMax = 5;
    }

    private void Start()
    {
        Player.OnDie += Player_OnDie;
        energyCellPostionArray = new Vector3[energyCellArray.Length];
        ammoPostionArray = new Vector3[ammoArray.Length];

        for (int i = 0; i < energyCellArray.Length; i++)
        {
            energyCellPostionArray[i] = energyCellArray[i].transform.position;
        }
        for (int i = 0; i < ammoArray.Length; i++)
        {
            ammoPostionArray[i] = ammoArray[i].transform.position;
        }
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
                isGameStarted = true;

                waveTimerMax = 30.0f * waveLevel;
                waveTimer = waveTimerMax;
                waveCounter++;

                OnGameStarted?.Invoke(this, EventArgs.Empty);
                OnWaveSpawned?.Invoke(this, new OnWaveSpawnedEventArgs { WaveCounter = waveCounter, WaveLevel = waveLevel });
            }

            OnStartingTimeChanged?.Invoke(this, timer);
        }

        if (isGameStarted)
        {
            gameTime += Time.deltaTime;

            OnGameTimeChanged?.Invoke(this, gameTime);

            waveTimer -= Time.deltaTime;

            OnWaveTimeChanged?.Invoke(this, waveTimer);

            if (waveTimer <= 0.0f)
            {
                waveTimer = waveTimerMax;
                waveCounter++;

                if (waveCounter >= waveCounterMax)
                {
                    waveCounter = 0;
                    waveLevel++;
                    waveTimerMax = 30.0f * waveLevel;
                    waveTimer = waveTimerMax;
                }

                OnWaveSpawned?.Invoke(this, new OnWaveSpawnedEventArgs { WaveCounter = waveCounter, WaveLevel = waveLevel });
            }
        }
    }

    private void EndGame()
    {
        Time.timeScale = 0.0f;

        environmentMusic.Stop();
    }

    private void Player_OnDie(object sender, EventArgs e)
    {
        EndGame();
        OnGameEnded?.Invoke(this, EventArgs.Empty);
    }

    private void ChargePoint_OnVehicleChargeCompleted(object sender, EventArgs e)
    {
        for (int i = 0; i < energyCellArray.Length; i++)
        {
            if (!energyCellArray[i])
            {
                EnergyCell energyCell = Instantiate(energyCellPrefab, energyCellPostionArray[i], Quaternion.identity, energyCellContainer).GetComponent<EnergyCell>();
                energyCellArray[i] = energyCell;
            }
        }
        for (int i = 0; i < ammoArray.Length; i++)
        {
            if (!ammoArray[i])
            {
                Ammo ammo = Instantiate(ammoPrefab, ammoPostionArray[i], Quaternion.identity, ammoContainer).GetComponent<Ammo>();
                ammoArray[i] = ammo;
            }
        }
    }

    public void StartGame()
    {
        isTimerStarted = true;

        environmentMusic.Play();
    }

    public void AddChargePoint(ChargePoint chargePoint)
    {
        chargePointList.Add(chargePoint);

        chargePoint.OnVehicleChargeCompleted += ChargePoint_OnVehicleChargeCompleted;
    }

    public void RemoveChargePoint(ChargePoint chargePoint)
    {
        chargePointList.Remove(chargePoint);
        if (chargePointList.Count <= 0)
        {
            EndGame();
            OnGameEnded?.Invoke(this, EventArgs.Empty);
        }
    }

    public List<ChargePoint> GetChargePointList() => chargePointList;
    public Player GetPlayer() => Player;
    public float GetGameTime() => gameTime;
    public float GetStartingGameTime() => timer;
    public int GetWaveLevel() => waveLevel;
    public float GetWaveTime() => waveTimer;
    public int GetWaveAmount() => waveCounter;
}
