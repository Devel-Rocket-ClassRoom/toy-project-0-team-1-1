using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "WeaponData", menuName = "Game/WeaponData")]
public class WeaponData : ScriptableObject, IUpgrade
{
    public string weaponName;
    public Sprite icon;
    public float damage;
    public float projectileSpeed;
    public float size;
    public float Range;
    public float cooldown;
    public int projectileCount;
    public int maxLevel = 8;
    public float existTime; // 지속시간
    public string Description;

    public List<LevelStats> levelStats;

    public string Name => weaponName;
    public Sprite Icon => icon;
    public UpgradeItemType type => UpgradeItemType.Weapon;

    public int Apply(PlayerStatus playerStatus, PlayerWeapon playerWeapon)
    {
        var weapon = playerWeapon.GetWeaponByData(this);
        if (weapon == null) return 0;
        if (weapon.Level >= maxLevel) return weapon.Level;

        var stats = levelStats[weapon.Level];
        for (int i = 0; i < stats.modifiers.Count; i++)
            weapon.AddModifier(stats.types[i], stats.modifiers[i]);

        weapon.LevelUp();
        return weapon.Level;
    }
}
