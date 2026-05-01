using UnityEngine;

public abstract class DirectionalWeaponBase : WeaponBase, IDirectionalAttackWeapon
{
    public Vector3 GetAttackDirection()
    {
        Transform target = FindNearestTarget();

        if (target == null)
            return transform.forward;

        Vector3 dir = target.position - transform.position;
        dir.y = 0f;

        if (dir.sqrMagnitude <= 0.001f)
            return transform.forward;

        return dir.normalized;
    }

    protected Transform FindNearestTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, Range, targetLayer);

        Transform nearest = null;
        float nearestDist = Mathf.Infinity;

        foreach (Collider hit in hits)
        {
            float dist = (hit.transform.position - transform.position).sqrMagnitude;

            if (dist < nearestDist)
            {
                nearestDist = dist;
                nearest = hit.transform;
            }
        }

        return nearest;
    }
}