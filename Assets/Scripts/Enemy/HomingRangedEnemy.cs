using UnityEngine;

public class HomingRangedEnemy : BaseEnemy
{
    [SerializeField]
    private GameObject projectilePrefab;

    protected override void Update()
    {
        if (isDead)
            return;

        float distance = Vector3.Distance(transform.position, _player.position);
        if (distance > attackDistance)
        {
            animator.SetBool("Run", true);
            Move();
        }
        else
        {
            animator.SetBool("Run", false);
            LookAtPlayer();
            DoAttak();
        }
    }

    protected override void DoAttak()
    {
        _attackTimer -= Time.deltaTime;
        if (_attackTimer > 0)
            return;

        _attackTimer = attackInterval;
        _agent.isStopped = true;
        _agent.ResetPath();
        _agent.velocity = Vector3.zero;

        animator.SetTrigger("Cast Spell");

        Vector3 spawnPos = transform.position + Vector3.up * 1f;
        Vector3 targetPos = new Vector3(
            _player.position.x,
            Mathf.Max(spawnPos.y, _player.position.y + 0.8f),
            _player.position.z
        );
        Vector3 dir = (targetPos - spawnPos).normalized;

        GameObject obj = PoolManager.Instance.Spawn(
            projectilePrefab,
            spawnPos,
            Quaternion.LookRotation(dir)
        );
        obj.GetComponent<HomingEnemyProjectile>().Init(projectilePrefab, Attack, dir);
    }
}
