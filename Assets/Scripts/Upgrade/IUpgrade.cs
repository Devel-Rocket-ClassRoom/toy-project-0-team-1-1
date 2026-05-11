using System.Collections;
using UnityEngine;

public interface IUpgrade
{
    UpgradeItemType type { get; }
    string Name { get; }
    Sprite Icon { get; }
    int Apply(PlayerStatus playerStatus, PlayerWeapon playerWeapon);
}

public enum UpgradeItemType
{
    Weapon,
    Passive,
}
