using UnityEngine;

public class TestWeaponStarter : MonoBehaviour
{
    [SerializeField] private WeaponBase startWeapon;

    private void Start()
    {
        startWeapon.Activate();
    }
}