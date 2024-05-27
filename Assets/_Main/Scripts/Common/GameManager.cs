using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Transform playerTransform;

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

    public Vector3 GetPlayerPosition() => playerTransform.position;
}
