using UnityEngine;

public class TripleRangedEnemy : BaseEnemy
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float spreadAngle = 15f; // 좌우로 벌어지는 각도(도 단위)
    protected override void Update()
    {
        var distance = Vector3.Distance(transform.position, _player.position);
        if (!isDead)
        {
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
    }
    protected override void DoAttak()
    {
        _attackTimer -= Time.deltaTime;
        if (_attackTimer > 0) return;
        _attackTimer = attackInterval;
        _agent.isStopped = true;
        _agent.ResetPath();
        _agent.velocity = Vector3.zero;
        animator.SetTrigger("Cast Spell");
        Vector3 spawnPos = transform.position + Vector3.up * 1f;
        Vector3 targetPos = new Vector3(_player.position.x, Mathf.Max(spawnPos.y, _player.position.y + 1f), _player.position.z);
        Vector3 centerDir = (targetPos - spawnPos).normalized;

        // -spreadAngle, 0, +spreadAngle 세 방향으로 발사
        float[] angles = { -spreadAngle, 0f, spreadAngle };
        foreach (float angle in angles)
        {
            Vector3 dir = Quaternion.Euler(0f, angle, 0f) * centerDir;
            GameObject obj = PoolManager.Instance.Spawn(
                projectilePrefab, spawnPos, Quaternion.LookRotation(dir));
            obj.GetComponent<EnemyProjectile>().Init(projectilePrefab, Attack, dir);
        }
    }
}