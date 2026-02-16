using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    private ItemDictionary itemDictionary;
    public GameObject inventoryUI;
    public GameObject slotPrefab;
    public int slotCount;
    public GameObject[] itemPrefabs;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        itemDictionary = FindAnyObjectByType<ItemDictionary>();
        
        // for (int i = 0; i < slotCount; i++)
        // {
        //     Slot slot = Instantiate(slotPrefab, inventoryUI.transform).GetComponent<Slot>();
        //     if (i < itemPrefabs.Length)
        //     {
        //         GameObject item = Instantiate(itemPrefabs[i], slot.transform);
        //         item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        //         slot.currentItem = item;
        //     }
        // }
    }

    public List<InventorySaveData> GetInventoryItems()
    {
        List<InventorySaveData> inventoryItems = new List<InventorySaveData>();
        foreach (Transform transform in inventoryUI.transform)
        {
            Slot slot = transform.GetComponent<Slot>();
            if (slot.currentItem != null)
            {
                Item item = slot.currentItem.GetComponent<Item>();
                inventoryItems.Add(new InventorySaveData { itemID = item.itemID, slotIndex = transform.GetSiblingIndex() });
            }
        }
        return inventoryItems;
    }

    public void SetInventoryItems(List<InventorySaveData> inventoryItems)
    {
        foreach (Transform child in inventoryUI.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < slotCount; i++)
        {
            Instantiate(slotPrefab, inventoryUI.transform);
        }
        
        foreach (InventorySaveData data in inventoryItems)
        {
            if(data.slotIndex < slotCount)
            {
                Slot slot = inventoryUI.transform.GetChild(data.slotIndex).GetComponent<Slot>();
                GameObject itemPrefab = itemDictionary.GetItemPrefab(data.itemID);
                if (itemPrefab != null)
                {
                    GameObject item = Instantiate(itemPrefab, slot.transform);
                    item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                    slot.currentItem = item;
                }
            }
        }
    }
}
