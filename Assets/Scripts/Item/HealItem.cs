using UnityEngine;

public class HealItem : Item
{
    public override void GetEffect(Transform player)
    {
        base.GetEffect(player);
        player.GetComponent<PlayerStatus>().Heal(itemData.effectValue);
    }
}
