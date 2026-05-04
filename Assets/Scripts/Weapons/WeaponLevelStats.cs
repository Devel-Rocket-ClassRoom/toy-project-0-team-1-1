using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponLevelStats
{
    public List<StatModifier> modifiers = new List<StatModifier>();
    public List<WeaponStatType> types = new List<WeaponStatType>();
    public string description;
}
