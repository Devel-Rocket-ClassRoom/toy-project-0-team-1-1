using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Game/WeaponData")]
public class WeaponData : ScriptableObject, IUpgrade
{
    public string weaponName;
    public Sprite icon;
    public float damage;
    public float speed;
    public float size;
    public float cooldown;
    public int projectileCount;
    public int maxLevel = 8;

    public List<LevelStats> levelStats;

    public string Name => weaponName;

    public Sprite Icon => icon;
    public UpgradeItemType type => UpgradeItemType.Weapon;
    public int Apply(PlayerStatus playerStatus, PlayerWeapon playerWeapon)
    {
        int currentLevel = playerWeapon.Weapons.ContainsKey(this) ? playerWeapon.Weapons[this].Level : 0;
        if (currentLevel < maxLevel)
        {
            LevelStats stats = levelStats[currentLevel];
            for (int i = 0; i < stats.modifiers.Count; i++)
            {
                StatModifier modifier = stats.modifiers[i];
                StatType type = stats.types[i];
                //playerWeapon.AddModifier(type, modifier);
            }
            playerWeapon.Weapons[this].LevelUp();
        }

        return playerWeapon.Weapons[this].Level;
    }
}