using UnityEngine;
using System.Collections;

public interface IUpgrade
{
    string Name { get; }
    Sprite Icon { get; }
    void Apply(PlayerStatus playerStatus, PlayerWeapon playerWeapon);
}