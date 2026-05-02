using UnityEngine;

public interface ILootable
{
    void StartLooting(Transform player);
    void GetEffect(Transform player);
}
