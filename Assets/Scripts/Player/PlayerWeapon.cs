using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private WeaponBase[] allWeapons;
    public Dictionary<WeaponData, WeaponBase> Weapons = new Dictionary<WeaponData, WeaponBase>();
    [SerializeField] private int maxWeaponCount = 6;
    public bool IsFull => Weapons.Count >= maxWeaponCount;

    private void Awake()
    {
        foreach (var weapon in allWeapons)
        {
            weapon.gameObject.SetActive(false);
        }
    }
    public void Equip(WeaponData weaponData, WeaponBase weapon)
    {
        if (IsFull) return;
        weapon.gameObject.SetActive(true);
        weapon.Activate();
        Weapons[weaponData] = weapon;
    }
}
