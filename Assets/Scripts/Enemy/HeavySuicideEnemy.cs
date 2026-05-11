using System.Collections;
using UnityEngine;

public class HeavySuicideEnemy : BaseEnemy
{
    [SerializeField]
    private GameObject explosionEffectPrefab;

    [SerializeField]
    private AudioClip bombClip;

    protected override void DoAttak()
    {
        if (IsDead)
            return;
        base.DoAttak();
        Die();
    }

    protected override void OnDie()
    {
        if (explosionEffectPrefab != null)
        {
            var explosion = Instantiate(
                explosionEffectPrefab,
                transform.position,
                Quaternion.identity
            );
            Destroy(explosion, 3f);
        }
        base.OnDie();
    }

    protected override IEnumerator DieRoutine()
    {
        yield return null;
        OnDie();
        yield return new WaitForSeconds(2f);
    }

    protected override void Die()
    {
        isDead = true;
        GetComponent<Collider>().enabled = false;
        StartCoroutine(DieRoutine());
    }
}
