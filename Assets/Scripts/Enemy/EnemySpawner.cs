using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public GameObject[] enemyPrefabs; // 이 웨이브에서 나올 몬스터 종류
        public int enemyCount;            // 총 스폰 수
        public float spawnInterval;       // 스폰 간격
    }

    [SerializeField] private Wave[] waves;
    [SerializeField] private float waveCooldown = 5f; // 웨이브 사이 간격
    [SerializeField] private float minSpawnDist = 15f;
    [SerializeField] private float spawnRangeFromPlayer = 40f;

    private int _currentWave = 0;
    private int _spawnedCount = 0;
    private float _timer;
    private bool _isWaveActive = false;
    private Transform _player;

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player").transform;
    }

    private void Start()
    {
        StartWave();
    }

    private void Update()
    {
        if (!_isWaveActive) return;

        _timer += Time.deltaTime;
        if (_timer >= waves[_currentWave].spawnInterval)
        {
            _timer = 0f;
            SpawnEnemy();
        }
    }

    private void StartWave()
    {
        if (_currentWave >= waves.Length) return;

        _spawnedCount = 0;
        _timer = 0f;
        _isWaveActive = true;
        Debug.Log($"웨이브 {_currentWave + 1} 시작");
    }

    private void SpawnEnemy()
    {
        Wave wave = waves[_currentWave];

        if (_spawnedCount >= wave.enemyCount)
        {
            _isWaveActive = false;
            _currentWave++;

            if (_currentWave < waves.Length)
            {
                Debug.Log($"{waveCooldown}초 후 웨이브 {_currentWave + 1} 시작");
                Invoke(nameof(StartWave), waveCooldown);
            }
            else
            {
                Debug.Log("모든 웨이브 완료");
            }
            return;
        }

        Vector3 spawnPos = GetSpawnPos();
        GameObject prefab = wave.enemyPrefabs[Random.Range(0, wave.enemyPrefabs.Length)];
        GameObject obj = PoolManager.Instance.Spawn(prefab, spawnPos, Quaternion.identity);

        NavMeshAgent agent = obj.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.Warp(spawnPos);
        }

        obj.GetComponent<BaseEnemy>().SetPrefab(prefab);
        _spawnedCount++;
    }

    private Vector3 GetSpawnPos()
    {
        Vector3 spawnPos;
        int maxAttempts = 20;

        do
        {
            Vector2 randomCircle = Random.insideUnitCircle * spawnRangeFromPlayer;
            Vector3 randomPos = _player.position + new Vector3(randomCircle.x, 0f, randomCircle.y);

            // NavMesh 위에서 가장 가까운 위치 찾기
            if (NavMesh.SamplePosition(randomPos, out NavMeshHit hit, 10f, NavMesh.AllAreas))
            {
                spawnPos = hit.position;
            }
            else
            {
                spawnPos = randomPos;
            }
            maxAttempts--;
        } while (Vector3.Distance(spawnPos, _player.position) < minSpawnDist && maxAttempts > 0);

        return spawnPos;
    }
}