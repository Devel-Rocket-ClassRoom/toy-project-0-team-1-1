using System.Collections.Generic;
using UnityEngine;

public class ShurikenOrbit : ProjectileBase
{
    [Header("Visual")]
    [SerializeField] private Transform visual;
    [SerializeField] private Vector3 visualRotationOffset = new Vector3(90f, 0f, 0f);

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
        GameObject prefab,
        float size)
    {
        base.Init(owner, direction, damage, speed, targetLayer, obstacleLayer, prefab);

        hitTargets.Clear();

        if (visual != null)
            visual.localRotation = Quaternion.Euler(visualRotationOffset);

        UpdateOrbitPosition();
    }

    public void SetOrbitData(float radius, float startAngle)
    {
        this.radius = radius;
        this.angle = startAngle;

        UpdateOrbitPosition();
    }

    private void Update()
    {
        if (owner == null)
        {
            Return();
            return;
        }

        angle += speed * Time.deltaTime;
        UpdateOrbitPosition();
    }

    private void UpdateOrbitPosition()
    {
        if (owner == null) return;

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
        if (!IsTarget(other)) return;
        if (hitTargets.Contains(other)) return;

        hitTargets.Add(other);
        Hit(other);
    }

    private void OnTriggerExit(Collider other)
    {
        hitTargets.Remove(other);
    }

    public void Return()
    {
        ReturnToPool();
    }
}