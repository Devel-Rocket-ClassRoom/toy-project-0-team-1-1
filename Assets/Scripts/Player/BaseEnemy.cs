using UnityEngine;

public abstract class BaseEnemy : BaseEntity
{
    protected StatContainer attack = new StatContainer(10f);
    [SerializeField] protected float attackDistance = 2f; 
    [SerializeField] protected float attackInterval = 1f;
    private float _attackTimer;

    public float Attack => attack.FinalValue;

    protected Transform player;
    [SerializeField] private LayerMask enemyLayer;

    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindWithTag("Player").transform;
    }

    protected override void InitStats()
    {
        maxHp = new StatContainer(50f);
        defense = new StatContainer(5f);
        speed = new StatContainer(5f);
        attack = new StatContainer(10f);
    }

    protected virtual void Update()
    {
        if (!isDead) Move();
    }

    protected virtual void Move()
    {
        var dir = (player.position - transform.position).normalized;
        var distance = Vector3.Distance(transform.position, player.position);

        if (distance > attackDistance)
        {
            animator.SetBool("Run", true);
            transform.position += dir * speed.FinalValue * Time.deltaTime;
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                Quaternion.LookRotation(dir),
                15f * Time.deltaTime
            );
        }
        else
        {
            animator.SetBool("Run", false);
            DoAttak();
        }
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
            playerStatus.TakeDamage(attack.FinalValue);
        }
    }

    protected override void Die()
    {
        animator.SetBool("Run", false);
        base.Die();
    }
}