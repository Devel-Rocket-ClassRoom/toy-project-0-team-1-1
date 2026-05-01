using UnityEngine;
using System.Collections;

public class ItemTest : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Collider[] items = Physics.OverlapSphere(transform.position, 100f, LayerMask.GetMask("Item"));
            Debug.Log($"Item Count : {items.Length}");
            foreach (var item in items)
            {
                ILootable lootableItem = item.GetComponent<ILootable>();
                if (lootableItem != null) lootableItem.StartLooting(transform);
            }
        }
    }
}