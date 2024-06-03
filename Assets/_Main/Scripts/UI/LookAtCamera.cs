using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] private bool isInverted = true;

    private Transform cameraTransform;

    private void Awake()
    {
        cameraTransform = Camera.main.transform;
    }

    private void LateUpdate()
    {
        if (isInverted)
        {
            Vector3 dirToCamera = (cameraTransform.position - transform.position).normalized;
            transform.LookAt(transform.position + dirToCamera * -0.1f);
        }
        else
        {
            transform.LookAt(cameraTransform);
        }
    }
}
