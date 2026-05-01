using UnityEngine;

public class RangedEnemy : BaseEnemy
{
    [SerializeField] private GameObject projectilePrefab; // 원거리 발사체 프리팹

    private void Awake()
    {
        base.Awake();
        attackDistance = 10f;
    }
    protected override void DoAttak()
    {
        _attackTimer -= Time.deltaTime;
        if (_attackTimer > 0) return;
        _attackTimer = attackInterval;

        var dir = (player.position - transform.position).normalized;
        GameObject obj = PoolManager.Instance.Spawn(projectilePrefab, transform.position, Quaternion.LookRotation(dir));
        obj.GetComponent<Projectile>().Init(projectilePrefab, Attack, dir);
    }
}
