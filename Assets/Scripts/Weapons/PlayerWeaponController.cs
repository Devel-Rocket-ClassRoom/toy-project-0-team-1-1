using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField] private List<WeaponBase> weapons = new List<WeaponBase>();

    private readonly Dictionary<WeaponBase, float> timers = new();

    private void Start()
    {
        foreach (var weapon in weapons)
        {
            if (weapon == null) continue;

            weapon.Activate();
            timers[weapon] = weapon.Cooldown;
        }
    }

    private void Update()
    {
        foreach (var weapon in weapons)
        {
            if (weapon == null || !weapon.IsActive) continue;

            timers[weapon] += Time.deltaTime;

            if (timers[weapon] >= weapon.Cooldown)
            {
                timers[weapon] = 0f;
                weapon.Use();
            }
        }
    }

    public void AddWeapon(WeaponBase weapon)
    {
        if (weapon == null || weapons.Contains(weapon)) return;

        weapons.Add(weapon);
        timers[weapon] = weapon.Cooldown;
        weapon.Activate();
    }
}