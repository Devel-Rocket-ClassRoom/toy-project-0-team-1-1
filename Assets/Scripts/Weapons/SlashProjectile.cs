using System.Collections.Generic;
using UnityEngine;

public class SlashProjectile : ProjectileBase
{
    [SerializeField] private float lifeTime = 0.25f;
    [SerializeField] private float swingAngle = 90f;
    [SerializeField] private float radius = 2f;

    private float timer;
    private Quaternion centerRotation;
    private readonly HashSet<Collider> damagedTargets = new();

    public override void Init(
        Transform owner,
        Vector3 direction,
        float damage,
        LayerMask targetLayer)
    {
        base.Init(owner, direction, damage, targetLayer);

        centerRotation = Quaternion.LookRotation(direction);
        transform.rotation = centerRotation * Quaternion.Euler(0f, -swingAngle * 0.5f, 0f);
    }

    private void Update()
    {
        timer += Time.deltaTime;

        float t = timer / lifeTime;
        float angle = Mathf.Lerp(-swingAngle * 0.5f, swingAngle * 0.5f, t);

        transform.rotation = centerRotation * Quaternion.Euler(0f, angle, 0f);

        if (owner != null)
        {
            transform.position = owner.position + transform.forward * radius;
        }

        if (timer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsTarget(other))
            return;

        if (damagedTargets.Contains(other))
            return;

        damagedTargets.Add(other);
        Hit(other);
    }
}