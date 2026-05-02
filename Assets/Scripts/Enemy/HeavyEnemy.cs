using UnityEngine;

public class HeavyEnemy : BaseEnemy
{
    private void Awake()
    {
        base.Awake();
        attackDistance = 4f;
    }
    protected override void InitStats()
    {
        stats[StatType.MaxHp] = new StatContainer(200f);
        stats[StatType.Speed] = new StatContainer(4f);
        stats[StatType.Defense] = new StatContainer(10f);
        stats[StatType.Attack] = new StatContainer(15f);
    }
}
