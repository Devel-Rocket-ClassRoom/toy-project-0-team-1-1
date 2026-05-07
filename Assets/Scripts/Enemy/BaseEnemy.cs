using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public abstract class BaseEnemy : BaseEntity
{
    [SerializeField] private EnemyData enemyData;
    protected float attackDistance;
    protected float attackInterval;
    protected float _attackTimer;
    [SerializeField] private GameObject expItemPrefab;
    [SerializeField] private GameObject damagePopupPrefab;
    [SerializeField] private AudioClip hitClip;
    [SerializeField] private AudioClip deathClip;
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
    protected override void OnEnable()
    {
        base.OnEnable();
        _agent.isStopped = false;
        foreach (var renderer in _renderers)
        {
            foreach (var mat in renderer.materials)
            {
                mat.SetColor("_EmissionColor", Color.black);
                mat.DisableKeyword("_EMISSION");
            }
        }
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = true;
        }
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
        stats[StatType.Resistance] = new StatContainer(enemyData.resistance);
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
        if (_agent == null || !_agent.enabled || !_agent.isOnNavMesh)
            return;
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
    protected override void Die()
    {
        _agent.isStopped = true;
        _agent.velocity = Vector3.zero;
        GetComponent<Collider>().enabled = false;
        SFXManager.Instance.Play3D(deathClip, transform.position);
        base.Die();
    }
    protected override void OnDie()
    {
        if (expItemPrefab != null)
        {
            var item = PoolManager.Instance.Spawn(expItemPrefab, transform.position, Quaternion.identity);
            //item.GetComponent<Item>().Init(transform.position);
        }
        if (_prefab != null)
        {
            PoolManager.Instance.Despawn(_prefab, gameObject);
        }
    }
    public override void TakeDamage(float damage)
    {
        if (IsDead) return;
        base.TakeDamage(damage);
        Vector3 pos = transform.position + Vector3.up * 2f;
        var popup = PoolManager.Instance.Spawn(damagePopupPrefab, pos, Quaternion.identity);
        popup.GetComponent<DamagePopup>().Setup((int)(Mathf.Max(0, damage - stats[StatType.Defense].FinalValue)));
        SFXManager.Instance.Play3D(hitClip, transform.position);
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
    protected override IEnumerator DieRoutine()
    {
        yield return new WaitForSeconds(1.5f);
        OnDie();
    }


    private Coroutine _knockbackCoroutine;

    public void KnockBack(float distance)
    {
        if (Resistance >= 100f) return;
        if (_knockbackCoroutine != null)
            StopCoroutine(_knockbackCoroutine);

        _knockbackCoroutine = StartCoroutine(KnockBackRoutine(distance));
    }

    private IEnumerator KnockBackRoutine(float baseDistance)
    {
        var resistance = Mathf.Max(0.000001f, 1f - Resistance / 100f);
        var distance = baseDistance * resistance;
        var dir = (transform.position - _player.transform.position).normalized;
        Vector3 targetPos = transform.position + dir * distance;

        if (_agent != null)
        {
            _agent.isStopped = true;
        }

        float elapsed = 0f;
        float duration = 0.2f * resistance;
        Vector3 startPos = transform.position;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            if (_agent != null)
            {
                _agent.Warp(Vector3.Lerp(startPos, targetPos, t));
            }

            yield return null;
        }

        if (_agent != null)
        {
            _agent.isStopped = false;
        }
    }
}