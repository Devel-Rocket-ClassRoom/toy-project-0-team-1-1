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
    public float damage;
    public float speed;
    public float cooldown;
    public int projectileCount;
    public float area;
    [TextArea] public string description;
}