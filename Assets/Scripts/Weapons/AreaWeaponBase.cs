using UnityEngine;

public abstract class AreaWeaponBase : WeaponBase, IAreaAttackWeapon
{
    public Collider[] FindTargetsInRange(Vector3 center, float radius)
    {
        return Physics.OverlapSphere(center, radius, targetLayer);
    }
}