using UnityEngine;
using UnityEngine.UI;

public class EnemyWorldUI : MonoBehaviour
{
    [SerializeField] private Image healthBarImage;
    [SerializeField] private HealthSystem healthSystem;

    private void Start()
    {
        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
    }

    private void HealthSystem_OnHealthChanged(object sender, float currentHealth)
    {
        UpdateHealthBar(currentHealth);
    }

    private void UpdateHealthBar(float currentHealth)
    {
        healthBarImage.fillAmount = currentHealth;
    }
}
