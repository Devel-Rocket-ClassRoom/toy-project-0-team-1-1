using System.Collections.Generic;
using UnityEngine;

public class ShurikenOrbit : ProjectileBase
{
    [Header("Visual")]
    [SerializeField] private Transform visual;
    [SerializeField] private Vector3 visualRotationOffset = new Vector3(90f, 0f, 0f);
    [SerializeField] private Vector3 spinAxis = Vector3.up;
    [SerializeField] private float spinSpeed = 720f;

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
        base.Init(owner, direction, damage, speed, targetLayer, obstacleLayer, prefab ,size);

        hitTargets.Clear();

        if (visual != null)
            visual.localRotation = Quaternion.Euler(visualRotationOffset);

        transform.localScale = Vector3.one * size;
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
        RotateVisual();
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

        BaseEnemy enemy = other.GetComponentInParent<BaseEnemy>();

        if (enemy == null || enemy.IsDead)
            return;

        hitTargets.Add(other);

        enemy.TakeDamage(damage);
    }
    private void RotateVisual()
    {
        if (visual == null)
            return;

        visual.Rotate(
            spinAxis,
            spinSpeed * Time.deltaTime,
            Space.Self
        );
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