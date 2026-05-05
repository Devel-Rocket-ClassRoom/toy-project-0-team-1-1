using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private int maxWeaponCount = 6;
    public Dictionary<WeaponData, WeaponBase> Weapons = new Dictionary<WeaponData, WeaponBase>();

    public bool IsFull => Weapons.Count >= maxWeaponCount;

    //테스트용
    [SerializeField] private WeaponData[] testWeaponDatas; // 인스펙터에서 연결

    private void Start()
    {
        if (testWeaponDatas != null)
        {
            foreach (var weaponData in testWeaponDatas)
            {
                Equip(weaponData);
            }
        }
    }
    //여기까지
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