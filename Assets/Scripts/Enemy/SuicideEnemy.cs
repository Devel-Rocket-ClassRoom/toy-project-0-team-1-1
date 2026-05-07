using System.Collections;
using UnityEngine;

public class SuicideEnemy : BaseEnemy
{
    [SerializeField] private GameObject explosionEffectPrefab;
    [SerializeField] private AudioClip bombClip;
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
        yield return null;
        OnDie();
    }
    protected override void Die()
    {
        isDead = true;
        GetComponent<Collider>().enabled = false;
        StartCoroutine(DieRoutine());
    }
}
