using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform[] enemyPrefabArray;

    private float spawnTimerMax;
    private float spawnTimer;
    private bool isTimerStarted;
    private int enemyAmount;

    private void Start()
    {
        GameManager.Instance.OnWaveSpawned += GameManager_OnWaveSpawned;
    }

    private void GameManager_OnWaveSpawned(object sender, GameManager.OnWaveSpawnedEventArgs e)
    {
        spawnTimerMax = UnityEngine.Random.Range(2.0f, 5.0f);
        spawnTimer = spawnTimerMax;
        enemyAmount = e.WaveLevel * 2;

        isTimerStarted = true;
    }

    private void Update()
    {
        if (isTimerStarted)
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0.0f)
            {
                spawnTimer = spawnTimerMax;
                Instantiate(enemyPrefabArray[UnityEngine.Random.Range(0, enemyPrefabArray.Length)], transform);

                enemyAmount--;
                if (enemyAmount <= 0)
                {
                    isTimerStarted = false;
                }
            }
        }
    }
}
