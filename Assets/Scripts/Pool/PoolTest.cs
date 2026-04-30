using System;
using UnityEngine;

public class PoolTest : MonoBehaviour
{
    [SerializeField] private GameObject prefab1;
    [SerializeField] private GameObject prefab2;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PoolManager.Instance.Spawn(prefab1, Vector3.zero, Quaternion.identity);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PoolManager.Instance.Spawn(prefab2, Vector3.zero, Quaternion.identity);
        }
    }
}
