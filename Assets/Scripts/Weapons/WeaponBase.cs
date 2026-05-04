using UnityEngine;

public abstract class WeaponBase : MonoBehaviour, IWeapon, IAutoAttackWeapon
{
    [Header("Base Stats")]
    [SerializeField] protected float baseDamage = 10f;
    [SerializeField] protected float baseCooldown = 1f;
    [SerializeField] protected float baseRange = 5f;

    [Header("Target")]
    [SerializeField] protected LayerMask targetLayer;

    [SerializeField] protected WeaponData weaponData;
    private int level = 1;
    protected bool isActive = false;

    protected virtual float Damage => baseDamage;
    public virtual float Cooldown => baseCooldown;
    public virtual float Range => baseRange;
    public virtual WeaponData WeaponData => weaponData;
    public int Level => level;
    //외부에서 상태 확인용
    public bool IsActive => isActive;

    public virtual void Activate()
    {
        isActive = true;
        OnActivate();
    }

    public virtual void Deactivate()
    {
        isActive = false;
        OnDeactivate();
    }

    protected virtual void OnActivate() { }
    protected virtual void OnDeactivate() { }

    //PlayerWeaponController가 호출하는 함수
    public void Use()
    {
        if (!isActive)
            return;

        Attack();
    }

    public abstract void Attack();
    public void LevelUp()
    {
        level++;
    }
}