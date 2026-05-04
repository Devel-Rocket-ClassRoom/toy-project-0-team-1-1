using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public abstract class BaseEnemy : BaseEntity
{
    [SerializeField] private EnemyData enemyData;
    protected float attackDistance;
    protected float attackInterval;
    protected float _attackTimer;
    [SerializeField] private GameObject expItemPrefab;
    private GameObject _prefab;
    public float Attack => stats[StatType.Attack].FinalValue;

    protected Transform _player;
    [SerializeField] private LayerMask enemyLayer;

    private NavMeshAgent _agent;

    private Renderer[] _renderers;
    private Coroutine _hitRoutine;

    protected override void Awake()
    {
        base.Awake();
        _player = GameObject.FindWithTag("Player").transform;
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = stats[StatType.Speed].FinalValue;
        _agent.stoppingDistance = attackDistance;

        _renderers = GetComponentsInChildren<Renderer>();
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

    protected virtual void Move()
    {
        animator.SetBool("Run", true);
        _agent.SetDestination(_player.position);
    }

    protected virtual void DoAttak()
    {
        _attackTimer -= Time.deltaTime;
        if (_attackTimer > 0)
        {
            return;
        }
        _attackTimer = attackInterval;

        var playerStatus = _player.GetComponent<PlayerStatus>();
        if (playerStatus != null)
        {
            playerStatus.TakeDamage(stats[StatType.Attack].FinalValue);
        }
    }

    protected virtual void LookAtPlayer()
    {
        Vector3 dir = (_player.position - transform.position).normalized;
        dir.y = 0f;
        if (dir.sqrMagnitude > 0.001f)
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), 15f * Time.deltaTime);
    }

    protected override void OnDie()
    {
        if (_prefab != null)
        {
            PoolManager.Instance.Despawn(_prefab, gameObject);
        }
        if (expItemPrefab != null)
        {
            PoolManager.Instance.Spawn(expItemPrefab, transform.position, Quaternion.identity);
        }
    }
    public override void TakeDamage(float damage)
    {
        if (IsDead) return;
        base.TakeDamage(damage);
        if (_hitRoutine != null)
        {
            StopCoroutine(_hitRoutine);
        }
        _hitRoutine = StartCoroutine(HitRoutine());
    }
    

    protected virtual IEnumerator HitRoutine()
    {
        foreach (var renderer in _renderers)
            foreach (var mat in renderer.materials)
            {
                mat.EnableKeyword("_EMISSION");
                mat.SetColor("_EmissionColor", Color.white);
            }

        yield return new WaitForSeconds(0.1f);

        foreach (var renderer in _renderers)
            foreach (var mat in renderer.materials)
            {
                mat.SetColor("_EmissionColor", Color.black);
                mat.DisableKeyword("_EMISSION");
            }
    }
}