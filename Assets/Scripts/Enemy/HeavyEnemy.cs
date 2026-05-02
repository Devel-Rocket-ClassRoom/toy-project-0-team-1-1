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
        maxHp = new StatContainer(200f);
        speed = new StatContainer(4f);
        defense = new StatContainer(10f);
        attack = new StatContainer(15f);
    }
}
