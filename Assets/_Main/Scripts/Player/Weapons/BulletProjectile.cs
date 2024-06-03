using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10.0f;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private Transform bulletHitVFXPrefab;

    private Vector3 targetDirection;
    private Gun gun;

    public void Setup(Vector3 targetDirection, Gun gun)
    {
        this.targetDirection = targetDirection;
        this.gun = gun;
    }

    private void Update()
    {
        transform.position += targetDirection * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        trailRenderer.transform.SetParent(null);

        if (other.GetComponent<Player>()) return;

        Vector3 hitPosition = other.TryGetComponent(out ITargatable targatable) ? targatable.GetTarget().position : other.transform.position;
        Instantiate(bulletHitVFXPrefab, hitPosition, Quaternion.identity);

        targatable.Hit(gun);

        Destroy(gameObject);
    }
}
