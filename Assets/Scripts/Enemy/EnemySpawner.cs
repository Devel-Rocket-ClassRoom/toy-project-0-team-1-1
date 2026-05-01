using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private float minSpawnDist = 15f; // 플레이어와 스폰위치의 최소 거리
    [SerializeField] private float mapWidth = 50f;   // 맵 가로 크기
    [SerializeField] private float mapHeight = 50f; // 맵 새로 크기

    private float _spawnTimer;
    private Transform _player;

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        _spawnTimer += Time.deltaTime;
        if(spawnInterval < _spawnTimer)
        {
            _spawnTimer = 0;
            SpawnEnemy();
        }
    }
    private void SpawnEnemy()
    {
        Vector3 spawnPos;
        int maxAttempts = 10; // 맵크기가 작아 발생하는 무한루프 방지

        do
        {
            float x = Random.Range(-mapWidth / 2f, mapWidth / 2f);
            float z = Random.Range(-mapHeight / 2f, mapHeight / 2f);
            spawnPos = new Vector3(x, 0f, z);
            maxAttempts--;
        } while (Vector3.Distance(spawnPos, _player.position) < minSpawnDist && maxAttempts > 0);
        GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        GameObject obj = PoolManager.Instance.Spawn(prefab, spawnPos, Quaternion.identity);
        obj.GetComponent<BaseEnemy>().SetPrefab(prefab);
    }
}
