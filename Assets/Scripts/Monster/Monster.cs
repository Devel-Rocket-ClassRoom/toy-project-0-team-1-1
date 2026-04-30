using UnityEngine;

public abstract class Monster : MonoBehaviour
{
    [SerializeField] protected float speed = 5f;
    [SerializeField] protected float attack = 10f;
    [SerializeField] protected float defense = 5f;
    [SerializeField] protected float resistance = 10f;
    [SerializeField] protected float maxHp = 100f;
    [SerializeField] protected float currentHp;
    [SerializeField] protected float attackDistance = 2f;
    protected bool isDead = false;

    protected Animator animator;
    protected Transform player;

    protected virtual void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        animator = GetComponent<Animator>();
        currentHp = maxHp;
    }

    protected virtual void Update()
    {
        if (!isDead)
        {
            Move();
        }
    }

    protected virtual void Move()
    {
        var dir = (player.position - transform.position).normalized;
        var distance = Vector3.Distance(transform.position, player.position);
        if (distance > attackDistance)
        {
            animator.SetBool("Run", true);
            transform.position += dir * speed * Time.deltaTime;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), 15f * Time.deltaTime);
        }
        else
        {
            animator.SetBool("Run", false);
        }
    }

    public virtual void TakeDamage(float damage)
    {
        currentHp -= damage;
        if (currentHp <= 0)
        {
            isDead = true;
            Die();
        }
    }
    public virtual void Die()
    {
        animator.SetBool("Run", false);
        animator.SetTrigger("Die");

    }





}
