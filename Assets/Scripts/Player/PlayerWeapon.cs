using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private int maxWeaponCount = 6;
    public Dictionary<WeaponData, WeaponBase> Weapons = new Dictionary<WeaponData, WeaponBase>();

    public bool IsFull => Weapons.Count >= maxWeaponCount;

    public void Equip(WeaponData data)
    {
        if (IsFull) return;
        GameObject obj = Instantiate(data.weaponPrefab, transform);
        WeaponBase weapon = obj.GetComponent<WeaponBase>();
        Weapons[data] = weapon;
        weapon.Activate();
    }

    public WeaponBase GetWeaponByData(WeaponData data) => Weapons.ContainsKey(data) ? Weapons[data] : null;
    public bool HasWeapon(WeaponData data) => Weapons.ContainsKey(data);
}