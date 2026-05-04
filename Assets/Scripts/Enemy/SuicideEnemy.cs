using System.Collections;
using UnityEngine;

public class SuicideEnemy : BaseEnemy
{
    [SerializeField] private GameObject explosionEffectPrefab;
    protected override void DoAttak()
    {
        if (IsDead) return;
        base.DoAttak();
        Debug.Log("자폭");
        Die();
    }
    protected override void OnDie()
    {
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }
        base.OnDie();
    }
    protected override IEnumerator DieRoutine()
    {
        OnDie();
        yield return null;
    }
    protected override void Die()
    {
        isDead = true;
        GetComponent<Collider>().enabled = false;
        StartCoroutine(DieRoutine());
    }
}
