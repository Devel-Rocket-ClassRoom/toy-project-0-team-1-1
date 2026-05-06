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
    public float range;
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

        Debug.Log($"{weaponName} 현재 레벨: {weapon.Level}, 최대 레벨: {maxLevel}");
        if (weapon.Level >= maxLevel) return weapon.Level;

        Debug.Log($"levelStats 개수: {levelStats.Count}, 현재 레벨: {weapon.Level}");
        var stats = levelStats[weapon.Level - 1];
        Debug.Log($"적용할 모디파이어 개수: {stats.modifiers.Count}");
        for (int i = 0; i < stats.modifiers.Count; i++)
        {
            Debug.Log($"모디파이어 적용: {stats.types[i]}, 값: {stats.modifiers[i].value}");
            weapon.AddModifier(stats.types[i], stats.modifiers[i]);
        }

        weapon.LevelUp();
        Debug.Log($"{weaponName} 레벨업 완료: {weapon.Level}");
        return weapon.Level;
    }
}
