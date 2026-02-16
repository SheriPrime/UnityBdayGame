using System.Collections.Generic;
using UnityEngine;

public class ItemDictionary : MonoBehaviour
{
    public List<Item> itemPrefabs;
    private Dictionary<int, GameObject> itemDictionary;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        itemDictionary = new Dictionary<int, GameObject>();
        for(int i = 0; i < itemPrefabs.Count; i++)
        {
            if (itemPrefabs[i] != null)
            {
                itemPrefabs[i].itemID = i + 1;
            }
            else
            {
                Debug.LogWarning($"Item prefab at index {i} is null. Please assign a valid Item prefab.");
            }

            foreach(Item item in itemPrefabs)
            {
                itemDictionary[item.itemID] = item.gameObject;
            }
        }
    }

    public GameObject GetItemPrefab(int itemID)
    {
        itemDictionary.TryGetValue(itemID, out GameObject prefab);
        if(prefab == null)
        {
            Debug.LogWarning($"Item with ID {itemID} not found in dictionary! Returning null.");
        }
        return prefab;
    }
}
