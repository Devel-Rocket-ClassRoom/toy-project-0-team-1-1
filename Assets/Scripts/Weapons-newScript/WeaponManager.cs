using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; private set; }

    [System.Serializable]
    public struct WeaponEntry
    {
        public WeaponData data;
        public GameObject prefab;
    }

    [SerializeField] private List<WeaponEntry> weaponEntries;
    public Dictionary<WeaponData, GameObject> Weapons { get; private set; }

    private void Awake()
    {
        Instance = this;
        Weapons = new();
        foreach (var entry in weaponEntries)
            Weapons[entry.data] = entry.prefab;
    }
}
