using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField] private WeaponBase startWeapon;
    [SerializeField] private List<WeaponBase> weapons = new List<WeaponBase>();

    private readonly Dictionary<WeaponBase, float> timers = new();

    private void Start()
    {
        foreach (var weapon in weapons)
        {
            if (weapon == null) continue;
            timers[weapon] = weapon.Cooldown;
        }

        if (startWeapon != null)
        {
            ActivateWeapon(startWeapon);
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

    public void ActivateWeapon(WeaponBase weapon)
    {
        if (weapon == null) return;

        if (!weapons.Contains(weapon))
            weapons.Add(weapon);

        if (!timers.ContainsKey(weapon))
            timers[weapon] = weapon.Cooldown;

        weapon.Activate();
    }
}