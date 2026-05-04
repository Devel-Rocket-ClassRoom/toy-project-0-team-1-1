using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    private Dictionary<WeaponData, WeaponBase> _weapons = new Dictionary<WeaponData, WeaponBase>();
    public Dictionary<WeaponData, WeaponBase> Weapons => _weapons;
    private int _maxWeaponCount = 6;
    public bool IsFull => _weapons.Count >= _maxWeaponCount;

    //public void Equip(GameObject weaponPrefab)
    //{
    //    if (IsFull)
    //    {
    //        return;
    //    }
    //    GameObject obj = Instantiate(weaponPrefab, transform);
    //    WeaponBase weapon = obj.GetComponent<WeaponBase>();
    //    _weapons.Add(weapon);
    //    weapon.Activate();
    //}
    
    public void Equip(WeaponData weaponData, WeaponBase weapon)
    {
        if (IsFull) return;
        if (_weapons.ContainsKey(weaponData)) return;
        _weapons.Add(weaponData, weapon);
        weapon.Activate();
    }
}
