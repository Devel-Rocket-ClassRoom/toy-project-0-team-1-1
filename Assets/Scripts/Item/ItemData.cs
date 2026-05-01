using UnityEngine;

public enum ItemEffectType
{
    Heal,
    ExpGain,
    Magnetic,
}

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public GameObject prefab;
    public float effectValue;
}
