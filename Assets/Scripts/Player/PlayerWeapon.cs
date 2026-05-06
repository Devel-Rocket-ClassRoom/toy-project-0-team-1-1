using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private int maxWeaponCount = 6;
    [SerializeField] private WeaponData defaultWeapon;
    public Dictionary<WeaponData, WeaponBase> Weapons = new Dictionary<WeaponData, WeaponBase>();

    public bool IsFull => Weapons.Count >= maxWeaponCount;

    //테스트용
    // [SerializeField] private WeaponData[] testWeaponDatas; // 인스펙터에서 연결

    private void Start()
    {
        Equip(defaultWeapon);

        if (UpgradeManager.Instance != null)
        {
            UpgradeManager.Instance.IconUpdate(defaultWeapon);
        }
        else
        {
            Debug.LogError("UpgradeManager.Instance가 null입니다.");
        }
    }
    //여기까지
    public void Equip(WeaponData weaponData)
    {
        if (IsFull) return;

        if (weaponData == null)
        {
            Debug.LogError("Equip 실패: weaponData가 null입니다. PlayerWeapon의 Default Weapon을 확인하세요.");
            return;
        }

        if (WeaponManager.Instance == null)
        {
            Debug.LogError("Equip 실패: WeaponManager.Instance가 null입니다. 씬에 WeaponManager가 있는지 확인하세요.");
            return;
        }

        if (WeaponManager.Instance.Weapons == null)
        {
            Debug.LogError("Equip 실패: WeaponManager.Instance.Weapons 딕셔너리가 null입니다.");
            return;
        }

        if (!WeaponManager.Instance.Weapons.ContainsKey(weaponData))
        {
            Debug.LogError($"Equip 실패: WeaponManager에 {weaponData.name} 무기가 등록되어 있지 않습니다.");
            return;
        }

        GameObject prefab = WeaponManager.Instance.Weapons[weaponData];

        if (prefab == null)
        {
            Debug.LogError($"Equip 실패: {weaponData.name}의 프리팹이 null입니다.");
            return;
        }

        GameObject obj = Instantiate(prefab, transform);

        WeaponBase weapon = obj.GetComponent<WeaponBase>();

        if (weapon == null)
        {
            Debug.LogError($"Equip 실패: {prefab.name} 프리팹에 WeaponBase 컴포넌트가 없습니다.");
            return;
        }

        Weapons[weaponData] = weapon;
        weapon.LevelUp();
        weapon.Activate();
    }

    public WeaponBase GetWeaponByData(WeaponData data) => Weapons.ContainsKey(data) ? Weapons[data] : null;
    public bool HasWeapon(WeaponData data) => Weapons.ContainsKey(data);
}