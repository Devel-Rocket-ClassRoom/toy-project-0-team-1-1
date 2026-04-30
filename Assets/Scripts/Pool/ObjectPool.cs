using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : Component
{
    private readonly T prefab;
    private readonly Transform parent;
    private readonly Queue<T> inActive = new Queue<T>();

    public ObjectPool(T prefab, Transform parent, int preLoadCount)
    {
        this.prefab = prefab;
        this.parent = parent;

        for (int i = 0; i < preLoadCount; i++)
        {
            var obj = Object.Instantiate(this.prefab, this.parent);
            obj.gameObject.SetActive(false);
            inActive.Enqueue(obj);
        }
    }

    public T Get()
    {
        T obj;
        if (inActive.Count > 0)
        {
            obj = inActive.Dequeue();
        }
        else
        {
            obj = Object.Instantiate(prefab, parent);
        }
        obj.gameObject.SetActive(true); // OnEnable 함수에서 초기화
        return obj;
    }
    
    public void Return(T obj)
    {
        obj.gameObject.SetActive(false);
        inActive.Enqueue(obj);
    }
}
