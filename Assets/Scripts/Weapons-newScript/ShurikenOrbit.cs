using System.Collections.Generic;
using UnityEngine;

public class ShurikenOrbit : ProjectileBase
{
    private float radius;
    private float angle;

    private readonly HashSet<Collider> hitTargets = new();

    public override void Init(
        Transform owner,
        Vector3 direction,
        float damage,
        float speed,
        LayerMask targetLayer,
        LayerMask obstacleLayer,
        GameObject prefab)
    {
        base.Init(owner, direction, damage, speed, targetLayer, obstacleLayer, prefab);

        hitTargets.Clear();
    }

    public void SetOrbitData(float radius, float startAngle)
    {
        this.radius = radius;
        this.angle = startAngle;
    }

    private void Update()
    {
        if (owner == null)
        {
            Return();
            return;
        }

        angle += speed * Time.deltaTime;

        float rad = angle * Mathf.Deg2Rad;

        Vector3 offset = new Vector3(
            Mathf.Cos(rad),
            0f,
            Mathf.Sin(rad)
        ) * radius;

        transform.position = owner.position + offset;
    }

    private void OnTriggerEnter(Collider other)
    {
        

        if (!IsTarget(other))
            return;
        Debug.Log($"충돌 감지됨: {other.name}");
        if (hitTargets.Contains(other))
            return;

        hitTargets.Add(other);

        Debug.Log($"타겟 맞음: {other.name}");

        Hit(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (hitTargets.Contains(other))
        {
            hitTargets.Remove(other);
        }
    }

    public void Return()
    {
        ReturnToPool();
    }
}