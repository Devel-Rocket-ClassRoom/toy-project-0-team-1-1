using NUnit.Framework.Interfaces;
using UnityEngine;

public class ExpItem : Item
{
    public override void GetEffect(Transform player)
    {
        base.GetEffect(player);
        player.GetComponent<PlayerLevel>().GainExp(itemData.effectValue);
    }
}
