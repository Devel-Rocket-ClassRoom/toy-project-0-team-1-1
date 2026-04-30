using UnityEngine;

public interface IAreaAttackWeapon
{
    float Range { get; }

    Collider[] FindTargetsInRange(Vector3 center, float radius);
}
