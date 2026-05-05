using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Random = UnityEngine.Random;
public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }
    [SerializeField] private List<UpgradeItemData> upgradeItemList = new List<UpgradeItemData>();     // 모든 업그레이드 아이템 데이터 리스트
    [SerializeField] private PlayerWeapon playerWeapon;                                               // 현재 플레이어가 장착한 무기 불러오기
    [SerializeField] private PlayerStatus playerStatus;                                               // 현재 플레이어의 업그레이드 아이템 불러오기
    [SerializeField] private PlayerLevel playerLevel;
    [SerializeField] private UpgradeUI upgradeUI;
    private List<WeaponData> weaponList = new List<WeaponData>();                                     // 모든 무기 데이터 리스트
    public event System.Action<IUpgrade> OnFirstGet;
    private void Awake()
    {
        Instance = this;
        playerLevel.OnLevelUp += ShowUpgradeSelection;
        upgradeUI.gameObject.SetActive(false);
    }
    private void Start()
    {
        weaponList = WeaponManager.Instance.Weapons.Keys.ToList();
    }
    public List<IUpgrade> GetRandomChoices(int count = 3)
    {
        var candidates = new List<IUpgrade>();

        for (int i = 0; i < count; i++)
        {
            float r = Random.value;

            if (r < 0.5f)
            {
                List<WeaponData> tempDataList = new List<WeaponData>();
                List<WeaponData> upgradableList = new List<WeaponData>();
                if (playerWeapon.IsFull)
                    tempDataList = playerWeapon.Weapons.Keys.ToList();
                else
                    tempDataList = weaponList;
                foreach (var weapon in tempDataList)
                {
                    if (!playerWeapon.HasWeapon(weapon))
                        upgradableList.Add(weapon);
                    else if (playerWeapon.Weapons[weapon].Level < weapon.maxLevel)
                        upgradableList.Add(weapon);
                }
                int index = Random.Range(0, upgradableList.Count);
                candidates.Add(upgradableList[index]);
            }
            else
            {
                List<UpgradeItemData> upgradableList = new List<UpgradeItemData>();
                foreach (var item in upgradeItemList)
                {
                    if (!playerStatus.upgradeItems.ContainsKey(item))
                        upgradableList.Add(item);
                    else if (playerStatus.upgradeItems[item] < item.maxLevel)
                        upgradableList.Add(item);
                }
                int index = Random.Range(0, upgradableList.Count);
                candidates.Add(upgradableList[index]);
            }
        }
        return candidates;
    }

    public void ShowUpgradeSelection()
    {
        var choices = GetRandomChoices(3);
        upgradeUI.Show(choices, ApplyUpgrade);
    }

    public void ApplyUpgrade(IUpgrade upgrade)
    {
        if (upgrade is WeaponData weapon)
        {
            if (!playerWeapon.HasWeapon(weapon))
            {
                playerWeapon.Equip(weapon);
            }
        }

        int level = upgrade.Apply(playerStatus, playerWeapon);
        if (level == 1) // 처음 획득하는 아이템일 경우
        {
            IconUpdate(upgrade);
        }
    }
    public void IconUpdate(IUpgrade upgrade)
    {
        OnFirstGet?.Invoke(upgrade);
    }

    public string GetItemDesc(IUpgrade upgrade) // 여기 작성
    {
        return "";
    }
}