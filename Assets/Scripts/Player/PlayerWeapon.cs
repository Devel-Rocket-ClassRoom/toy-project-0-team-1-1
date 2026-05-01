using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    private List<WeaponBase> _weapons = new List<WeaponBase>();
    private int _maxWeaponCount;
    public bool IsFull => _weapons.Count > _maxWeaponCount;
    public void Equip(GameObject weaponPrefab)
    {
        if (IsFull)
        {
            return;
        }
        GameObject obj = Instantiate(weaponPrefab, transform);
        WeaponBase weapon = obj.GetComponent<WeaponBase>();
        _weapons.Add(weapon);
        weapon.Activate();
    }
}
