using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class EnemySpawner2 : MonoBehaviour
{
    [System.Serializable]
    public class EnemySpawnInfo
    {
        public GameObject prefab;
        public int count;
    }

    [System.Serializable]
    public class Wave
    {
        public EnemySpawnInfo[] enemies;  // 프리팹별 스폰 수
        public float spawnInterval;
        public int chestCount = 1;

        public int TotalEnemyCount
        {
            get
            {
                int total = 0;
                for (int i = 0; i < enemies.Length; i++)
                    total += enemies[i].count;
                return total;
            }
        }
    }

    [SerializeField] private Wave[] waves;
    [SerializeField] private float waveCooldown = 5f;
    [SerializeField] private float minSpawnDist = 15f;
    [SerializeField] private float spawnRangeFromPlayer = 40f;

    [SerializeField] private BreakableObject treasureChestData;

    private int _currentWave = 0;
    private int _spawnedCount = 0;
    private int _spawnedChestCount = 0;
    private int[] _spawnedCountPerType;   // 타입별 스폰 수 추적
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
        _spawnedChestCount = 0;  // 웨이브 전환 시 상자 카운터도 리셋
        _spawnedCountPerType = new int[waves[_currentWave].enemies.Length];
        _timer = 0f;
        _isWaveActive = true;
        Debug.Log($"웨이브 {_currentWave + 1} 시작");
    }

    private void SpawnEnemy()
    {
        Wave wave = waves[_currentWave];
        if (_spawnedCount >= wave.TotalEnemyCount)
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

        // 아직 한도가 남은 타입 중에서 선택
        int selectedIndex = SelectRandomAvailableEnemy(wave);
        if (selectedIndex < 0) return;

        Vector3 spawnPos = GetSpawnPos();
        if (spawnPos == Vector3.positiveInfinity) return;

        GameObject prefab = wave.enemies[selectedIndex].prefab;
        GameObject obj = PoolManager.Instance.Spawn(prefab, spawnPos, Quaternion.identity);

        NavMeshAgent agent = obj.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.Warp(spawnPos);
        }

        obj.GetComponent<BaseEnemy>().SetPrefab(prefab);
        _spawnedCountPerType[selectedIndex]++;
        _spawnedCount++;
    }

    /// <summary>
    /// 남은 한도가 있는 타입 중에서 균등 랜덤 선택. 할당 없는 2-pass 방식.
    /// </summary>
    private int SelectRandomAvailableEnemy(Wave wave)
    {
        int availableCount = 0;
        for (int i = 0; i < wave.enemies.Length; i++)
        {
            if (_spawnedCountPerType[i] < wave.enemies[i].count)
                availableCount++;
        }
        if (availableCount == 0) return -1;

        int target = Random.Range(0, availableCount);
        int counted = 0;
        for (int i = 0; i < wave.enemies.Length; i++)
        {
            if (_spawnedCountPerType[i] < wave.enemies[i].count)
            {
                if (counted == target) return i;
                counted++;
            }
        }
        return -1;
    }

    private void TrySpawnChest()
    {
        Wave wave = waves[_currentWave];

        if (_spawnedChestCount >= wave.chestCount) return;

        float progress = (float)_spawnedCount / wave.TotalEnemyCount;
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

            if (NavMesh.SamplePosition(randomPos, out NavMeshHit hit, 10f, NavMesh.AllAreas))
            {
                spawnPos = hit.position;

                if (Vector3.Distance(spawnPos, _player.position) >= minSpawnDist)
                {
                    return spawnPos;
                }
            }

            maxAttempts--;
        } while (maxAttempts > 0);

        if (NavMesh.SamplePosition(_player.position, out NavMeshHit fallbackHit, 30f, NavMesh.AllAreas))
        {
            return fallbackHit.position;
        }

        return Vector3.positiveInfinity;
    }
}
