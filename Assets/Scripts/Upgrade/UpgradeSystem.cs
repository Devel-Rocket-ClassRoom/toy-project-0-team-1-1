using UnityEngine;

public static class UpgradeSystem
{
    public static void WeaponUpgrade(WeaponData weaponData, WeaponBase weapon, int level)
    {
        if (level <= weaponData.maxLevel)
        {
            WeaponLevelStats levelStats = weaponData.levelStats[level];
            for (int i = 0; i < levelStats.modifiers.Count; i++)
            {
                // weapon.AddModifier(levelStats.types[i], levelStats.modifiers[i]);
            }
        }
    }
    public static void PlayerUpgrade(UpgradeItemData upgradeItem, PlayerStatus player, int level)
    {
        if (level <= upgradeItem.maxLevel)
        {
            ItemLevelStats levelStats = upgradeItem.levelStats[level];
            for (int i = 0; i < levelStats.modifiers.Count; i++)
            {
                player.AddModifier(levelStats.types[i], levelStats.modifiers[i]);
            }
        }
    }
}
