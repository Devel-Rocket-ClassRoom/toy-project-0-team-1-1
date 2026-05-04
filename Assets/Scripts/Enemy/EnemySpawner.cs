using UnityEngine;

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
    [SerializeField] private float mapWidth = 50f;
    [SerializeField] private float mapHeight = 50f;

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
        obj.GetComponent<BaseEnemy>().SetPrefab(prefab);
        _spawnedCount++;
    }

    private Vector3 GetSpawnPos()
    {
        Vector3 spawnPos;
        int maxAttempts = 10;

        do
        {
            float x = Random.Range(-mapWidth / 2f, mapWidth / 2f);
            float z = Random.Range(-mapHeight / 2f, mapHeight / 2f);
            spawnPos = new Vector3(x, 0f, z);
            maxAttempts--;
        } while (Vector3.Distance(spawnPos, _player.position) < minSpawnDist && maxAttempts > 0);

        return spawnPos;
    }
}