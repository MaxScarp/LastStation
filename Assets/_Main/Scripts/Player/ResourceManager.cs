using System;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    public event EventHandler<int> OnEnergyCellAmountChanged;

    [SerializeField] private int EnergyCellAmountMax = 5;

    private int energyCellAmountCurrent;

    private void Awake()
    {
        if (Instance)
        {
            Debug.LogError($"Error: There is more than one {transform} in the scene!");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public bool TryUseEnergyCell()
    {
        if (energyCellAmountCurrent > 0)
        {
            energyCellAmountCurrent--;

            OnEnergyCellAmountChanged?.Invoke(this, energyCellAmountCurrent);

            return true;
        }

        return false;
    }

    public bool TryAddEnergyCell()
    {
        if (energyCellAmountCurrent < EnergyCellAmountMax)
        {
            energyCellAmountCurrent++;

            OnEnergyCellAmountChanged?.Invoke(this, energyCellAmountCurrent);

            return true;
        }

        return false;
    }

    public int GetCurrentEnergyCellAmount() => energyCellAmountCurrent;
}
