using System.Collections.Generic;
using UnityEngine;

public class SlashProjectile : ProjectileBase
{
    [Header("Slash")]
    [SerializeField] private float lifeTime = 0.25f;
    [SerializeField] private float radius = 2f;
    [SerializeField] private float swingAngle = 90f;

    private float timer;
    private Quaternion baseRotation;
    private readonly HashSet<Collider> hitTargets = new();

    public override void Init(
        Transform owner,
        Vector3 direction,
        float damage,
        float speed,
        LayerMask targetLayer,
        LayerMask obstacleMask,
        GameObject prefab,
        float size)
    {
        base.Init(owner, direction, damage, speed, targetLayer, obstacleMask, prefab);

        timer = 0f;
        hitTargets.Clear();

        baseRotation = Quaternion.LookRotation(direction);
        transform.rotation = baseRotation * Quaternion.Euler(0f, -swingAngle * 0.5f, 0f);

        if (owner != null)
        {
            transform.position = owner.position + transform.forward * radius;
        }
    }

    private void Update()
    {
        if (owner == null)
        {
            ReturnToPool();
            return;
        }

        timer += Time.deltaTime;

        float t = timer / lifeTime;
        float angle = Mathf.Lerp(-swingAngle * 0.5f, swingAngle * 0.5f, t);

        transform.rotation = baseRotation * Quaternion.Euler(0f, angle, 0f);
        transform.position = owner.position + transform.forward * radius;

        if (timer >= lifeTime)
        {
            ReturnToPool();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsTarget(other))
            return;

        if (hitTargets.Contains(other))
            return;

        hitTargets.Add(other);
        Hit(other);
    }
}