using UnityEngine;

public class EliteRangedEnemy : BaseEnemy
{
    [SerializeField] private GameObject projectilePrefab; // 원거리 발사체 프리팹
    protected override void Update()
    {
        var distance = Vector3.Distance(transform.position, _player.position);
        if (!isDead)
        {
            if (distance > attackDistance)
            {
                animator.SetBool("Fly Idle", false);
                animator.SetBool("Fly Forward", true);
                Move();
            }
            else
            {
                animator.SetBool("Fly Forward", false);
                animator.SetBool("Fly Idle", true);
                LookAtPlayer();
                DoAttak();
            }
        }
    }
    protected override void Move()
    {
        if (_agent == null || !_agent.enabled || !_agent.isOnNavMesh)
            return;
        animator.SetBool("Fly Forward", true);
        _agent.SetDestination(_player.position);
    }
    protected override void DoAttak()
    {
        _attackTimer -= Time.deltaTime;
        if (_attackTimer > 0) return;
        _attackTimer = attackInterval;
        _agent.isStopped = true;
        _agent.ResetPath();
        _agent.velocity = Vector3.zero;
        animator.SetTrigger("Fly Cast Spell 01");
        Vector3 spawnPos = transform.position + Vector3.up * 1f;
        Vector3 targetPos = new Vector3(_player.position.x, Mathf.Max(spawnPos.y, _player.position.y + 1f), _player.position.z);
        var dir = (targetPos - spawnPos).normalized;
        GameObject obj = PoolManager.Instance.Spawn(projectilePrefab, spawnPos, Quaternion.LookRotation(dir));
        obj.GetComponent<EnemyProjectile>().Init(projectilePrefab, Attack, dir);

    }
}
