using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private Image healtBar;
    [SerializeField] private Player player;

    [SerializeField] private TextMeshProUGUI energyCellAmountText;
    [SerializeField] private TextMeshProUGUI ammoAmountText;
    [SerializeField] private TextMeshProUGUI gameTimeText;
    [SerializeField] private TextMeshProUGUI gameStartingTimeText;
    [SerializeField] private TextMeshProUGUI nextWaveTimeText;
    [SerializeField] private TextMeshProUGUI waveAmountText;

    private void Start()
    {
        player.OnDamageTaken += Player_OnDamageTaken;
        player.GetGun().OnShoot += Player_Gun_OnShoot;
        ResourceManager.Instance.OnEnergyCellAmountChanged += ResourceManager_OnEnergyCellAmountChanged;
        player.OnAmmoTaken += Player_OnAmmoTaken;
        GameManager.Instance.OnGameTimeChanged += GameManager_OnGameTimeChanged;
        GameManager.Instance.OnStartingTimeChanged += GameManager_OnStartingTimeChanged;
        GameManager.Instance.OnGameStarted += GameManager_OnGameStarted;
        GameManager.Instance.OnWaveSpawned += GameManager_OnWaveSpawned;
        GameManager.Instance.OnWaveTimeChanged += GameManager_OnWaveTimeChanged;

        UpdateHealthBar(player.GetCurrentHealth());
        UpdateEnergyCellText(ResourceManager.Instance.GetCurrentEnergyCellAmount());
        UpdateAmmoText(player.GetGun().GetCurrentAmmo());
        UpdateGameTimeText(GameManager.Instance.GetGameTime());
        UpdateGameStartingTimeText(GameManager.Instance.GetStartingGameTime());
        UpdateNextWaveTime(GameManager.Instance.GetWaveTime());
        UpdateWaveAmountText(GameManager.Instance.GetWaveAmount());
    }

    private void GameManager_OnWaveTimeChanged(object sender, float time)
    {
        UpdateNextWaveTime(time);
    }

    private void GameManager_OnWaveSpawned(object sender, GameManager.OnWaveSpawnedEventArgs e)
    {
        UpdateWaveAmountText(e.WaveCounter);
    }

    private void GameManager_OnGameStarted(object sender, EventArgs e)
    {
        gameStartingTimeText.gameObject.SetActive(false);
    }

    private void GameManager_OnStartingTimeChanged(object sender, float time)
    {
        UpdateGameStartingTimeText(time);
    }

    private void GameManager_OnGameTimeChanged(object sender, float gameTime)
    {
        UpdateGameTimeText(gameTime);
    }

    private void Player_OnAmmoTaken(object sender, EventArgs e)
    {
        UpdateAmmoText(player.GetGun().GetCurrentAmmo());
    }

    private void Player_Gun_OnShoot(object sender, int currentAmmoAmount)
    {
        UpdateAmmoText(currentAmmoAmount);
    }

    private void ResourceManager_OnEnergyCellAmountChanged(object sender, int currentEnergyCellAmount)
    {
        UpdateEnergyCellText(currentEnergyCellAmount);
    }

    private void Player_OnDamageTaken(object sender, float currentHealth)
    {
        UpdateHealthBar(currentHealth);
    }

    private void UpdateWaveAmountText(int waveAmount)
    {
        waveAmountText.text = $"Wave amount: {waveAmount}";
    }

    private void UpdateNextWaveTime(float time)
    {
        nextWaveTimeText.text = $"Next wave in: {time:00.00}";
    }

    private void UpdateGameTimeText(float gameTime)
    {
        gameTimeText.text = $"Game Time: {gameTime:00.00}";
    }

    private void UpdateGameStartingTimeText(float time)
    {
        gameStartingTimeText.text = $"Game Time: {time:00}";
    }

    private void UpdateHealthBar(float currentHealth)
    {
        healtBar.fillAmount = currentHealth;
    }

    private void UpdateEnergyCellText(int currentEnergyCellAmount)
    {
        energyCellAmountText.text = $"Energy Cells: {currentEnergyCellAmount}";
    }

    private void UpdateAmmoText(int currentAmmoAmount)
    {
        ammoAmountText.text = $"Ammo: {currentAmmoAmount}";
    }
}
