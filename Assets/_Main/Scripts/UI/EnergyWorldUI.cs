using UnityEngine;
using UnityEngine.UI;

public class EnergyWorldUI : MonoBehaviour
{
    [SerializeField] private Image barImage;
    [SerializeField] private Vehicle vehicle;

    private void Start()
    {
        vehicle.OnChargeChanged += Vehicle_OnChargeChanged;

        UpdateEnergyBar(vehicle.GetCurrentChargeNormalized());
    }

    private void Vehicle_OnChargeChanged(object sender, float currentCharge)
    {
        UpdateEnergyBar(currentCharge);
    }

    private void UpdateEnergyBar(float currentCharge)
    {
        barImage.fillAmount = currentCharge;
    }
}
