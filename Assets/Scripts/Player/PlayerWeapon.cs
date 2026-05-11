using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField]
    private int maxWeaponCount = 6;

    //[SerializeField] private WeaponData defaultWeapon;
    public Dictionary<WeaponData, WeaponBase> Weapons = new Dictionary<WeaponData, WeaponBase>();

    public bool IsFull => Weapons.Count >= maxWeaponCount;

    //�׽�Ʈ��
    // [SerializeField] private WeaponData[] testWeaponDatas; // �ν����Ϳ��� ����

    private void Start()
    {
        //Equip(defaultWeapon);

        //if (UpgradeManager.Instance != null)
        //{
        //    UpgradeManager.Instance.IconUpdate(defaultWeapon);
        //}
        //else
        //{
        //    Debug.LogError("UpgradeManager.Instance�� null�Դϴ�.");
        //}
    }

    //�������
    public void Equip(WeaponData weaponData)
    {
        if (IsFull)
            return;

        if (weaponData == null)
        {
            Debug.LogError(
                "Equip ����: weaponData�� null�Դϴ�. PlayerWeapon�� Default Weapon�� Ȯ���ϼ���."
            );
            return;
        }

        if (WeaponManager.Instance == null)
        {
            Debug.LogError(
                "Equip ����: WeaponManager.Instance�� null�Դϴ�. ���� WeaponManager�� �ִ��� Ȯ���ϼ���."
            );
            return;
        }

        if (WeaponManager.Instance.Weapons == null)
        {
            Debug.LogError("Equip ����: WeaponManager.Instance.Weapons ��ųʸ��� null�Դϴ�.");
            return;
        }

        if (!WeaponManager.Instance.Weapons.ContainsKey(weaponData))
        {
            Debug.LogError(
                $"Equip ����: WeaponManager�� {weaponData.name} ���Ⱑ ��ϵǾ� ���� �ʽ��ϴ�."
            );
            return;
        }

        GameObject prefab = WeaponManager.Instance.Weapons[weaponData];

        if (prefab == null)
        {
            Debug.LogError($"Equip ����: {weaponData.name}�� �������� null�Դϴ�.");
            return;
        }

        GameObject obj = Instantiate(prefab, transform);

        WeaponBase weapon = obj.GetComponent<WeaponBase>();

        if (weapon == null)
        {
            Debug.LogError($"Equip ����: {prefab.name} �����տ� WeaponBase ������Ʈ�� �����ϴ�.");
            return;
        }

        Weapons[weaponData] = weapon;
        var modifiers = this.GetComponent<PlayerStatus>().WeaponModifiers;
        foreach (var modifier in modifiers)
        {
            Weapons[weaponData].AddModifier(modifier.type, modifier.mod);
        }
        weapon.LevelUp();
        weapon.Activate();
    }

    public WeaponBase GetWeaponByData(WeaponData data) =>
        Weapons.ContainsKey(data) ? Weapons[data] : null;

    public bool HasWeapon(WeaponData data) => Weapons.ContainsKey(data);

    public void DeactivateAllWeapons()
    {
        foreach (var weapon in Weapons.Values)
        {
            if (weapon != null)
            {
                weapon.Deactivate();
            }
        }
    }
}
