using UnityEngine;

public class RangedEnemy : BaseEnemy
{
    [SerializeField] private GameObject projectilePrefab; // 원거리 발사체 프리팹

    protected override void DoAttak()
    {
        _attackTimer -= Time.deltaTime;
        if (_attackTimer > 0) return;
        _attackTimer = attackInterval;

        Vector3 spawnPos = transform.position + Vector3.up * 1f;
        Vector3 targetPos = new Vector3(_player.position.x, spawnPos.y, _player.position.z);
        var dir = (targetPos - spawnPos).normalized;
        GameObject obj = PoolManager.Instance.Spawn(projectilePrefab, spawnPos, Quaternion.LookRotation(dir));
        obj.GetComponent<EnemyProjectile>().Init(projectilePrefab, Attack, dir);

    }
}
