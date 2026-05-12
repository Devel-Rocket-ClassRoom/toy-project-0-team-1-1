using System.Collections.Generic;
using UnityEngine;

public class WhetstoneItem : Item
{
    public override void GetEffect(Transform player)
    {
        base.GetEffect(player);

        PlayerStatus playerStatus = player.GetComponent<PlayerStatus>();

        // 보유 패시브 아이템 중 레벨 1 이상인 것 레벨업
        List<UpgradeItemData> upgradeItemKeys = new List<UpgradeItemData>(
            playerStatus.upgradeItems.Keys
        );
        foreach (UpgradeItemData upgradeItemData in upgradeItemKeys)
        {
            int currentLevel = playerStatus.upgradeItems[upgradeItemData];
            if (currentLevel >= 1 && currentLevel < upgradeItemData.maxLevel)
            {
                LevelStats stats = upgradeItemData.levelStats[currentLevel];
                for (int i = 0; i < stats.modifiers.Count; i++)
                {
                    playerStatus.AddModifier(stats.types[i], stats.modifiers[i]);
                }
                playerStatus.upgradeItems[upgradeItemData] = currentLevel + 1;
            }
        }
    }
}
