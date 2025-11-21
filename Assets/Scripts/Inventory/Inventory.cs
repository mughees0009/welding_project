using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public InventoryType inventoryType;
    [Header("UI")]
    public Transform content;
    public InventoryItemUI inventoryItemUIPrefab;

    [Header("Inventory Data")]
    public List<InventoryItemInfo> items = new();
    public int capacity = 12;

    private readonly List<InventoryItemUI> currItems = new();

    public virtual bool AddItem(InventoryItemInfo itemInfo)
    {
        if (items.Count >= capacity)
            return false;

        items.Add(itemInfo);

        InventoryItemUI uiItem = Instantiate(inventoryItemUIPrefab, content);
        uiItem.Init(itemInfo.icon, inventoryType, itemInfo);

        currItems.Add(uiItem);

        return true;
    }

    public virtual bool RemoveItem(InventoryItemInfo itemInfo)
    {
        int index = items.IndexOf(itemInfo);
        if (index < 0)
            return false;

        items.RemoveAt(index);

        if (index < currItems.Count)
        {
            Destroy(currItems[index].gameObject);
            currItems.RemoveAt(index);
        }

        return true;
    }
}

public enum InventoryType
{
    Player,
    Storage
}
