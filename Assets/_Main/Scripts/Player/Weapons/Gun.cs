using System;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    public event EventHandler OnAmmoFinished;
    public event EventHandler<int> OnShoot;

    [SerializeField] private int maxAmmo = 30;
    [SerializeField] private Transform bulletPrefab;
    [SerializeField] private Transform shootPointTransform;
    [SerializeField] private Transform bulletContainer;
    [SerializeField] private float damage = 0.25f;

    [SerializeField] private AudioSource shootAudioSource;

    private int currentAmmo;

    private void Start()
    {
        Hide();
    }

    private void InputManager_OnShootPerformed(object sender, EventArgs e)
    {
        Fire();
    }

    private void Hide()
    {
        InputManager.Instance.OnShootPerformed -= InputManager_OnShootPerformed;

        gameObject.SetActive(false);
    }

    private void Fire()
    {
        currentAmmo--;

        OnShoot?.Invoke(this, currentAmmo);

        shootAudioSource.Play();

        if (currentAmmo <= 0)
        {
            Hide();

            OnAmmoFinished?.Invoke(this, EventArgs.Empty);

            return;
        }

        Vector3 aimPoint = InputManager.Instance.GetMousePosition();
        aimPoint.y = shootPointTransform.position.y;
        Vector3 shoorDirection = (aimPoint - shootPointTransform.position).normalized;
        BulletProjectile bullet = Instantiate(bulletPrefab, shootPointTransform.position, Quaternion.identity, bulletContainer).GetComponent<BulletProjectile>();
        bullet.Setup(shoorDirection, this);
    }

    public void Show()
    {
        gameObject.SetActive(true);

        currentAmmo = maxAmmo;

        InputManager.Instance.OnShootPerformed += InputManager_OnShootPerformed;
    }

    public float GetDamage() => damage;
    public int GetCurrentAmmo() => currentAmmo;
}
