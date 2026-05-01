using UnityEngine;

public class SuicideEnemy : BaseEnemy
{
    protected override void InitStats()
    {
        maxHp = new StatContainer(50f);
        speed = new StatContainer(12f);
        defense = new StatContainer(0f);
        attack = new StatContainer(50f);
    }
    protected override void DoAttak()
    {
        if (IsDead) return;
        base.DoAttak();
        Debug.Log("자폭");
        Die();
    }
}
