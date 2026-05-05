using UnityEngine;
using System.Collections;

public class MagneticItem : Item
{
    private float areaSize = 100f;
    private float duration = 5f;
    private Coroutine coMagnetic = null;
    public override void GetEffect(Transform player)
    {
        if (coMagnetic != null) StopCoroutine(coMagnetic);
        coMagnetic = StartCoroutine(CoMagneticRoutine(player));
    }
    private IEnumerator CoMagneticRoutine(Transform player)
    {
        var status = player.GetComponent<PlayerStatus>();
        status.AddModifier(StatType.LootingArea, new StatModifier(ModType.Flat, areaSize, this));
        yield return new WaitForSeconds(duration);
        status.RemoveBySource(StatType.LootingArea, this);
    }
}