using NUnit.Framework.Interfaces;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Random = UnityEngine.Random;
public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }
    [SerializeField] private List<WeaponData> weaponList = new List<WeaponData>();                    // 모든 무기 데이터 리스트
    [SerializeField] private List<UpgradeItemData> upgradeItemList = new List<UpgradeItemData>();     // 모든 업그레이드 아이템 데이터 리스트
    [SerializeField] private PlayerWeapon playerWeapon;                                               // 현재 플레이어가 장착한 무기 불러오기
    [SerializeField] private PlayerStatus playerStatus;                                               // 현재 플레이어의 업그레이드 아이템 불러오기
    [SerializeField] private PlayerLevel playerLevel;
    [SerializeField] private UpgradeUI upgradeUI;

    private void Awake()
    {
        playerLevel.OnLevelUp += ShowUpgradeSelection;
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
                    if (playerWeapon.Weapons[weapon].Level < weapon.maxLevel)
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
                    if (playerStatus.upgradeItems[item] < item.maxLevel)
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
        upgrade.Apply(playerStatus, playerWeapon);
    }
}