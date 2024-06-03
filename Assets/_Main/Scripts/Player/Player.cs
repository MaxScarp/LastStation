using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, ITargatable
{
    public event EventHandler OnAmmoTaken;
    public event EventHandler OnAmmoFinished;
    public event EventHandler<float> OnDamageTaken;
    public event EventHandler OnDie;

    [SerializeField] private Transform target;
    [SerializeField] private Gun gun;
    [SerializeField] private SkinnedMeshRenderer visualMeshRenderer;
    [SerializeField] private Material hitMaterial;

    [SerializeField] private float maxHitTimer = 0.75f;

    private List<ChargePoint> chargePointList;
    private HealthSystem healthSystem;
    private PlayerMovement playerMovement;

    private bool isHit;
    private bool isHitMaterialSetted;
    private float hitTimer;

    private Material regularMaterial;

    private void Awake()
    {
        chargePointList = new List<ChargePoint>();

        healthSystem = GetComponent<HealthSystem>();
        playerMovement = GetComponent<PlayerMovement>();

        hitTimer = maxHitTimer;
    }

    private void Start()
    {
        GameManager.Instance.Player = this;

        InputManager.Instance.OnInteractPerformed += Instance_OnInteractPerformed;
        gun.OnAmmoFinished += Gun_OnAmmoFinished;
        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
        InputManager.Instance.OnFillPerformed += InputManager_OnFillPerformed;
    }

    private void Update()
    {
        HandleHit();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ChargePoint chargePoint))
        {
            chargePointList.Add(chargePoint);
        }
        else if (other.TryGetComponent(out Ammo ammo))
        {
            if (gun.GetCurrentAmmo() > 0) return;

            gun.Show();
            ammo.Destroy();

            OnAmmoTaken?.Invoke(this, EventArgs.Empty);
        }
        else if (other.TryGetComponent(out EnergyCell energyCell))
        {
            if (ResourceManager.Instance.TryAddEnergyCell())
            {
                energyCell.Destroy();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out ChargePoint chargePoint))
        {
            chargePointList.Remove(chargePoint);
        }
    }

    private void InputManager_OnFillPerformed(object sender, EventArgs e)
    {
        foreach (ChargePoint chargePoint in chargePointList)
        {
            chargePoint.Fill();
        }
    }

    private void HealthSystem_OnHealthChanged(object sender, float currentHealth)
    {
        OnDamageTaken?.Invoke(this, currentHealth);
        if (healthSystem.GetCurrentHealth() <= 0.0f)
        {
            OnDie?.Invoke(this, EventArgs.Empty);
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

    private void Gun_OnAmmoFinished(object sender, EventArgs e)
    {
        OnAmmoFinished?.Invoke(this, EventArgs.Empty);
    }

    private void Instance_OnInteractPerformed(object sender, EventArgs e)
    {
        foreach (ChargePoint chargePoint in chargePointList)
        {
            chargePoint.Interact();
        }
    }

    public void TakeDamage(float damage)
    {
        healthSystem.TakeDamage(damage);
    }

    public void Hit(Gun gun = null)
    {
        isHit = true;
    }

    public Transform GetTarget() => target;
    public PlayerMovement GetPlayerMovement() => playerMovement;
    public Gun GetGun() => gun;
    public float GetCurrentHealth() => healthSystem.GetCurrentHealth();
}
