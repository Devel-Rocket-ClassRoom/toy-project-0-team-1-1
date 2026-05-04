using UnityEngine.UI;
using UnityEngine;

public class UIInventorySlot : MonoBehaviour
{
    [SerializeField] private Image[] weaponIcons;
    [SerializeField] private Image[] passiveIcons;
    private int weaponCount;
    private int passiveCount;
    private void Awake()
    {
        for (int i = 0; i < weaponIcons.Length; i++) weaponIcons[weaponCount++].enabled = false;
        for (int i = 0; i < passiveIcons.Length; i++) passiveIcons[passiveCount++].enabled = false;
        UpgradeManager.Instance.OnFirstGet += InventorySlotUpdate;
        weaponCount = 0;
        passiveCount = 0;
    }
    public void InventorySlotUpdate(IUpgrade item)
    {
        if (item.type == UpgradeItemType.Weapon)
        {
            weaponIcons[weaponCount++].sprite = item.Icon;
            weaponIcons[weaponCount++].enabled = true;
        }
        else if (item.type == UpgradeItemType.Passive)
        {
            passiveIcons[passiveCount++].sprite = item.Icon;
            passiveIcons[passiveCount++].enabled = true;
        }
    }
}
