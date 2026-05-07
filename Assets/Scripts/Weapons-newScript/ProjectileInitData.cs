using UnityEngine;

public struct ProjectileInitData
{
    public Transform owner;
    public Vector3 direction;
    public float damage;
    public float speed;
    public LayerMask targetLayer;
    public LayerMask obstacleLayer;
    public GameObject prefab;
    public float size;
    public float knockBack;
}
