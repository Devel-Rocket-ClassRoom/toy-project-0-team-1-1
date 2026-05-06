using System.Collections.Generic;
using UnityEngine;

public class ShurikenTrap : ProjectileBase
{
    [Header("Trap")]
    [SerializeField] private float tickInterval = 0.5f;

    [Header("Visual")]
    [SerializeField] private Transform visual;
    [SerializeField] private Vector3 rotateAxis = Vector3.up;
    [SerializeField] private float rotateSpeed = 180f;

    private float duration;
    private float timer;

    private readonly Dictionary<BaseEnemy, float> targets = new();

    public void InitTrap(
        Transform owner,
        float damage,
        float duration,
        float size,
        LayerMask targetLayer,
        LayerMask obstacleLayer,
        GameObject prefab)
    {
        Init(
            owner,
            Vector3.zero,
            damage,
            0f,
            targetLayer,
            obstacleLayer,
            prefab
        );

        this.duration = duration;
        timer = 0f;

        targets.Clear();

        transform.localScale = Vector3.one * size;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= duration)
        {
            Return();
            return;
        }

        RotateVisual();
        UpdateDamage();
    }

    private void RotateVisual()
    {
        if (visual == null)
            return;

        visual.Rotate(
            rotateAxis,
            rotateSpeed * Time.deltaTime,
            Space.Self
        );
    }

    private void UpdateDamage()
    {
        List<BaseEnemy> targetList = new List<BaseEnemy>(targets.Keys);

        foreach (BaseEnemy enemy in targetList)
        {
            if (enemy == null || enemy.IsDead)
            {
                targets.Remove(enemy);
                continue;
            }

            if (!targets.ContainsKey(enemy))
                continue;

            targets[enemy] -= Time.deltaTime;

            if (targets[enemy] <= 0f)
            {
                enemy.TakeDamage(damage);
                targets[enemy] = tickInterval;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsTarget(other))
            return;

        BaseEnemy enemy = other.GetComponentInParent<BaseEnemy>();

        if (enemy == null || enemy.IsDead)
            return;

        if (!targets.ContainsKey(enemy))
            targets.Add(enemy, 0f);
    }

    private void OnTriggerExit(Collider other)
    {
        BaseEnemy enemy = other.GetComponentInParent<BaseEnemy>();

        if (enemy == null)
            return;

        targets.Remove(enemy);
    }

    public void Return()
    {
        targets.Clear();
        ReturnToPool();
    }
}