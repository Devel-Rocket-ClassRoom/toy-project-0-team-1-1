using UnityEngine;

public class SuicideEnemy : BaseEnemy
{
    protected override void InitStats()
    {
        stats[StatType.MaxHp] = new StatContainer(50f);
        stats[StatType.Speed] = new StatContainer(12f);
        stats[StatType.Defense] = new StatContainer(0f);
        stats[StatType.Attack] = new StatContainer(50f);
    }
    protected override void DoAttak()
    {
        if (IsDead) return;
        base.DoAttak();
        Debug.Log("자폭");
        Die();
    }
}
