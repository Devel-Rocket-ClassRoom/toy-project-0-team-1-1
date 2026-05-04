using UnityEngine;

public class SuicideEnemy : BaseEnemy
{
    protected override void DoAttak()
    {
        if (IsDead) return;
        base.DoAttak();
        Debug.Log("자폭");
        Die();
    }
}
