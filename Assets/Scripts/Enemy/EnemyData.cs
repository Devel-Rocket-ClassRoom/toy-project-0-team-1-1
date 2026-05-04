using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Game/EnemyData")]
public class EnemyData : ScriptableObject
{
    public float maxHp = 50f;
    public float defense = 5f;
    public float speed = 5f;
    public float attack = 10f;
    public float attackDistance = 2f;
    public float attackInterval = 1f;
}
