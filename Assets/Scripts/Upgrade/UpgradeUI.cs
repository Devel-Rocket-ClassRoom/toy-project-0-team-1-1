using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private UpgradeSlotUI[] slots;
    [SerializeField] private GameObject player;
    private PlayerStatus playerStatus;
    private PlayerWeapon playerWeapon;

    private System.Action<IUpgrade> onSelected;

    private void Awake()
    {
        playerStatus = player.GetComponent<PlayerStatus>();
        playerWeapon = player.GetComponent<PlayerWeapon>();
    }
    public void Show(List<IUpgrade> choices, System.Action<IUpgrade> onSelected)
    {
        this.onSelected = onSelected;

        Time.timeScale = 0f;
        panel.SetActive(true);

        for (int i = 0; i < slots.Length; i++)
        {
            if (i < choices.Count)
            {
                slots[i].gameObject.SetActive(true);
                var upgrade = choices[i];
                if (upgrade is WeaponData weapon)
                {
                    if (playerWeapon.HasWeapon(weapon))
                    {
                        var thisWeapon = playerWeapon.GetWeaponByData(weapon);
                        slots[i].Setup(upgrade, OnSlotSelected, weapon.levelStats[thisWeapon.Level].description, $"Lv.{thisWeapon.Level}");
                    }
                    else
                    {
                        slots[i].Setup(upgrade, OnSlotSelected, "신규획득", "Lv.0");
                    }
                }
                else if (upgrade is UpgradeItemData itemData)
                {
                    if (playerStatus.upgradeItems.ContainsKey(itemData))
                    {
                        slots[i].Setup(upgrade, OnSlotSelected, itemData.levelStats[playerStatus.upgradeItems[itemData]].description, $"Lv.{playerStatus.upgradeItems[itemData]}");
                    }
                    else
                    {
                        slots[i].Setup(upgrade, OnSlotSelected, "신규획득", "Lv.0");
                    }
                }
                else
                {
                    slots[i].Setup(upgrade, OnSlotSelected, "", "");
                }
            }
            else
            {
                slots[i].gameObject.SetActive(false);
            }
        }
    }

    private void OnSlotSelected(IUpgrade upgrade)
    {
        panel.SetActive(false); // 패널 닫기
        Time.timeScale = 1f;

        foreach (var slot in slots) // 모든 슬롯 정리
            slot.Clear();

        onSelected?.Invoke(upgrade); // 선택 결과 전달
    }
}