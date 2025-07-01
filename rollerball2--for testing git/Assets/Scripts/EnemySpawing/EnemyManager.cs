using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _enemyPrefab;

    [SerializeField]
    private float _minSpawnTime;

    [SerializeField]
    private float _maxSpawnTime;

    [SerializeField]
    private Transform playerTransform; // drag player here via Inspector

    private float _timeUntilSpawn;

    public int enemyLimit = 3;
    private int count = 0;

    void Awake()
    {
        SetTimeUntilSpawn();

        // Fallback auto-assign if not set
        if (playerTransform == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                playerTransform = playerObj.transform;
            }
            else
            {
                Debug.LogWarning("PlayerTransform not set and Player not found by tag!");
            }
        }
    }

    public void NotifyEnemyDestroyed()
    {
        count--;
    }

    void Update()
    {
        _timeUntilSpawn -= Time.deltaTime;
        if (_timeUntilSpawn < 0 && count < enemyLimit)
        {
            GameObject enemy = Instantiate(
                _enemyPrefab[Random.Range(0, _enemyPrefab.Length)],
                transform.position,
                Quaternion.identity
            );

            // Check for multiple script types
            var enemyScript1 = enemy.GetComponent<randomAItest>();
            var enemyScript2 = enemy.GetComponent<Bill_AI>();

            if (enemyScript1 != null)
            {
                enemyScript1.SetManager(this);
                enemyScript1.SetTarget(playerTransform);
            }

            if (enemyScript2 != null)
            {
                enemyScript2.SetManager(this);
                enemyScript2.SetTarget(playerTransform);
            }

            count++;
            SetTimeUntilSpawn();
        }
    }

    void SetTimeUntilSpawn()
    {
        _timeUntilSpawn = Random.Range(_minSpawnTime, _maxSpawnTime);
    }
}
