using NUnit.Framework.Interfaces;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Random = UnityEngine.Random;
public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }
    [SerializeField] private List<UpgradeItemData> upgradeItemList = new List<UpgradeItemData>(); // 모든 업그레이드 아이템 데이터 리스트
    [SerializeField] private PlayerWeapon playerWeapon; // 현재 플레이어가 장착한 무기 불러오기
    [SerializeField] private PlayerStatus playerStatus; // 현재 플레이어의 업그레이드 아이템 불러오기
    [SerializeField] private PlayerLevel playerLevel;
    [SerializeField] private UpgradeUI upgradeUI;
    [SerializeField] private UIPlayerHealthBar hpBar;
    private List<WeaponData> weaponList = new List<WeaponData>(); // 모든 무기 데이터 리스트
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
        var allCandidates = new List<IUpgrade>();

        List<WeaponData> weaponSource = playerWeapon.IsFull
            ? playerWeapon.Weapons.Keys.ToList()
            : weaponList;

        foreach (var weapon in weaponSource)
        {
            if (!playerWeapon.HasWeapon(weapon))
                allCandidates.Add(weapon);
            else if (playerWeapon.Weapons[weapon].Level < weapon.maxLevel)
                allCandidates.Add(weapon);
        }

        foreach (var item in upgradeItemList)
        {
            if (!playerStatus.upgradeItems.ContainsKey(item))
                allCandidates.Add(item);
            else if (playerStatus.upgradeItems[item] < item.maxLevel)
                allCandidates.Add(item);
        }

        int actualCount = Mathf.Min(count, allCandidates.Count);

        for (int i = allCandidates.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (allCandidates[i], allCandidates[j]) = (allCandidates[j], allCandidates[i]);
        }

        return allCandidates.GetRange(0, actualCount);
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
                IconUpdate(upgrade);
            }
        }
        else
        {
            upgrade.Apply(playerStatus, playerWeapon);
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