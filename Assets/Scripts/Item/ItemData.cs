using UnityEngine;

public enum ItemEffectType
{
    Heal,
    ExpGain,
    Magnetic,
}

[CreateAssetMenu(fileName = "ItemData", menuName = "Game/ItemData")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public float effectValue;
    public GameObject prefab;
}
