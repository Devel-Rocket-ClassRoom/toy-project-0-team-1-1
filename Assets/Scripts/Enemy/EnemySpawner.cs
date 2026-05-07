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
        public int chestCount = 1;        // 웨이브당 상자 갯수
    }

    [SerializeField] private Wave[] waves;
    [SerializeField] private float waveCooldown = 5f; // 웨이브 사이 간격
    [SerializeField] private float minSpawnDist = 15f;
    [SerializeField] private float spawnRangeFromPlayer = 40f;

    [SerializeField] private BreakableObject treasureChestData;

    private int _currentWave = 0;
    private int _spawnedCount = 0;
    private int _spawnedChestCount = 0; // 생성된 상자 수
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
            TrySpawnChest();
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
        if (spawnPos == Vector3.positiveInfinity) return;// 유효하지 않으면 스폰 안 함
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
    private void TrySpawnChest()
    {
        Wave wave = waves[_currentWave];

        if (_spawnedChestCount >= wave.chestCount) return;

        // 웨이브 진행도에 따라 상자 스폰
        float progress = (float)_spawnedCount / wave.enemyCount;
        int expectedChests = Mathf.FloorToInt(progress * wave.chestCount);

        if (_spawnedChestCount < expectedChests)
        {
            SpawnChest();
            _spawnedChestCount++;
        }
    }
    private void SpawnChest()
    {
        if (treasureChestData == null || treasureChestData.prefab == null) return;

        Vector3 spawnPos = GetSpawnPos();
        PoolManager.Instance.Spawn(treasureChestData.prefab, spawnPos, Quaternion.identity);
    }


    private Vector3 GetSpawnPos()
    {
        Vector3 spawnPos = _player.position;
        int maxAttempts = 20;

        do
        {
            Vector2 randomCircle = Random.insideUnitCircle * spawnRangeFromPlayer;
            Vector3 randomPos = _player.position + new Vector3(randomCircle.x, 0f, randomCircle.y);

            // NavMesh 위에서 가장 가까운 위치 찾기
            if (NavMesh.SamplePosition(randomPos, out NavMeshHit hit, 10f, NavMesh.AllAreas))
            {
                spawnPos = hit.position;

                // 거리 조건 만족하면 바로 리턴
                if (Vector3.Distance(spawnPos, _player.position) >= minSpawnDist)
                {
                    return spawnPos;
                }
            }

            maxAttempts--;
        } while (maxAttempts > 0);

        // 실패하면 플레이어 근처 NavMesh 위치 강제 검색
        if (NavMesh.SamplePosition(_player.position, out NavMeshHit fallbackHit, 30f, NavMesh.AllAreas))
        {
            return fallbackHit.position;
        }

        return Vector3.positiveInfinity;
    }
}