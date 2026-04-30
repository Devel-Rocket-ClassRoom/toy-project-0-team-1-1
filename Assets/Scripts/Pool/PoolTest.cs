using System.Collections.Generic;
using UnityEngine;

public class PoolTest : MonoBehaviour
{
    [SerializeField] private GameObject prefab1;
    [SerializeField] private GameObject prefab2;
    Queue<GameObject> pools1 = new Queue<GameObject>();
    Queue<GameObject> pools2 = new Queue<GameObject>();
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            pools1.Enqueue(PoolManager.Instance.Spawn(prefab1, Vector3.zero, Quaternion.identity));
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            pools2.Enqueue(PoolManager.Instance.Spawn(prefab2, Vector3.zero, Quaternion.identity));
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (pools1.Count > 0)
                PoolManager.Instance.Despawn(prefab1, pools1.Dequeue());
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (pools2.Count > 0)
                PoolManager.Instance.Despawn(prefab2, pools2.Dequeue());
        }
    }
}
