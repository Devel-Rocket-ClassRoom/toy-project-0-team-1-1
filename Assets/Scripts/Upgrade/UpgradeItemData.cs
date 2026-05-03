using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeItemData", menuName = "Game/UpgradeItemData")]
public class UpgradeItemData : ScriptableObject, IUpgrade
{
    public Sprite icon;
    public string itemName;
    public int maxLevel = 5;
    public List<LevelStats> levelStats = new List<LevelStats>();

    public string Name => itemName;
    public Sprite Icon => icon;

    public void Apply(PlayerStatus playerStatus, PlayerWeapon playerWeapon)
    {
        int currentLevel = playerStatus.upgradeItems.ContainsKey(this) ? playerStatus.upgradeItems[this] : 0;
        if (currentLevel < maxLevel)
        {
            LevelStats stats = levelStats[currentLevel];
            for (int i = 0; i < stats.modifiers.Count; i++)
            {
                StatModifier modifier = stats.modifiers[i];
                StatType type = stats.types[i];
                playerStatus.AddModifier(type, modifier);
            }
            playerStatus.upgradeItems[this] = currentLevel + 1;
        }
    }
}

[System.Serializable]
public class LevelStats
{
    public List<StatModifier> modifiers = new List<StatModifier>();
    public List<StatType> types = new List<StatType>();
    public string description;
}