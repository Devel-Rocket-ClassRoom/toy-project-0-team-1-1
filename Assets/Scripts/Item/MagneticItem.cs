using UnityEngine;
using System.Collections;

public class MagneticItem : Item
{
    private float areaSize = 100f;
    private float duration = 5f;
    private Coroutine coMagnetic = null;
    public override void GetEffect(Transform player)
    {
        var status = player.GetComponent<PlayerStatus>();
        if (coMagnetic != null)
        {
            status.StopCoroutine(coMagnetic);
            status.RemoveBySource(StatType.LootingArea, itemData);
        }
        coMagnetic = status.StartCoroutine(CoMagneticRoutine(status));
    }
    private IEnumerator CoMagneticRoutine(PlayerStatus status)
    {
        status.AddModifier(StatType.LootingArea, new StatModifier(ModType.Flat, areaSize, itemData));
        yield return new WaitForSeconds(duration);
        status.RemoveBySource(StatType.LootingArea, itemData);
    }
}