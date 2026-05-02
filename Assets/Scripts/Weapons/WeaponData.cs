using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Game/WeaponData")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public GameObject prefab;
    public int maxLevel = 8;

    public List<WeaponLevelStats> levelStats;
}
[System.Serializable]
public class WeaponLevelStats
{
    public List<StatModifier> modifiers = new List<StatModifier>();
    public List<StatType> types = new List<StatType>();
    public string description;
}