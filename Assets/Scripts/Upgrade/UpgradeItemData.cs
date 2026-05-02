using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeItemData", menuName = "Game/UpgradeItemData")]
public class UpgradeItemData : ScriptableObject
{
    public Sprite icon;
    public string itemName;
    public int maxLevel = 5;

    public List<ItemLevelStats> levelStats = new List<ItemLevelStats>();
}

[System.Serializable]
public class ItemLevelStats
{
    public List<StatModifier> modifiers = new List<StatModifier>();
    public List<StatType> types = new List<StatType>();
    public string description;
}