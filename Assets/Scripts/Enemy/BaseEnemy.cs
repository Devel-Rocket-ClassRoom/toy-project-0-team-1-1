using UnityEngine;

public abstract class BaseEnemy : BaseEntity
{
    [SerializeField] private EnemyData enemyData;
    protected float attackDistance;
    protected float attackInterval;
    protected float _attackTimer;
    [SerializeField] private GameObject expItemPrefab;
    private GameObject _prefab;
    public float Attack => stats[StatType.Attack].FinalValue;

    protected Transform player;
    [SerializeField] private LayerMask enemyLayer;

    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindWithTag("Player").transform;
    }
    public void SetPrefab(GameObject prefab) => _prefab = prefab;

    protected override void InitStats()
    {
        if (enemyData == null)
        {
            Debug.LogError($"{gameObject.name}에 EnemyData가 없어요");
            return;
        }
        stats[StatType.MaxHp] = new StatContainer(enemyData.maxHp);
        stats[StatType.Defense] = new StatContainer(enemyData.defense);
        stats[StatType.Speed] = new StatContainer(enemyData.speed);
        stats[StatType.Attack] = new StatContainer(enemyData.attack);
        attackDistance = enemyData.attackDistance;
        attackInterval = enemyData.attackInterval;
    }

    protected virtual void Update()
    {
        var distance = Vector3.Distance(transform.position, player.position);
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
                DoAttak();
            }
        }
    }

    protected virtual void Move()
    {
        var dir = (player.position - transform.position).normalized;
        animator.SetBool("Run", true);
        transform.position += dir * stats[StatType.Speed].FinalValue * Time.deltaTime;
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            Quaternion.LookRotation(dir),
            15f * Time.deltaTime
            );
        Separate();
    }
    private void Separate()
    {
        Collider[] neighbors = Physics.OverlapSphere(transform.position, 1f, enemyLayer);
        foreach (var neighbor in neighbors)
        {
            if (neighbor.gameObject == gameObject) continue;
            Vector3 pushDir = transform.position - neighbor.transform.position;
            transform.position += pushDir.normalized * 0.05f;
        }
    }

    protected virtual void DoAttak()
    {
        _attackTimer -= Time.deltaTime;
        if (_attackTimer > 0)
        {
            return;
        }
        _attackTimer = attackInterval;

        var playerStatus = player.GetComponent<PlayerStatus>();
        if (playerStatus != null)
        {
            playerStatus.TakeDamage(stats[StatType.Attack].FinalValue);
        }
    }

    protected override void Die()
    {
        animator.SetBool("Run", false);
        base.Die();
        if (_prefab != null)
        {
            PoolManager.Instance.Despawn(_prefab, gameObject);
        }
        if (expItemPrefab != null)
        {
            PoolManager.Instance.Spawn(expItemPrefab, transform.position, Quaternion.identity);
        }
    }
}