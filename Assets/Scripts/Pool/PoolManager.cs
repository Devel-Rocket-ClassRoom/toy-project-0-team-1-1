using UnityEngine;
using System.Collections.Generic;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }
    
    [System.Serializable]
    public struct PoolConfig
    {
        public GameObject prefab;
        public int preloadCount;
    }
    
    [SerializeField] private PoolConfig[] configs;
    
    private Dictionary<GameObject, ObjectPool<Transform>> pools = new();
    
    void Awake()
    {
        Instance = this;
        foreach (var config in configs)
        {
            var pool = new ObjectPool<Transform>(
                config.prefab.transform,
                transform,
                config.preloadCount
            );
            pools[config.prefab] = pool;
        }
    }
    
    public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (!pools.ContainsKey(prefab))
        {
            pools[prefab] = new ObjectPool<Transform>(
                prefab.transform, transform, 0
            );
        }
        
        var obj = pools[prefab].Get();
        obj.position = position;
        obj.rotation = rotation;
        return obj.gameObject;
    }
    
    public void Despawn(GameObject prefab, GameObject obj)
    {
        pools[prefab].Return(obj.transform);
    }
}
