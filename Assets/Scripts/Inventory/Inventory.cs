using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class Inventory : MonoBehaviour
{
    public InventoryType inventoryType;

    [Header("UI")]
    public Transform content;                     // Parent container for UI items
    public InventoryItemUI inventoryItemUIPrefab; // UI prefab for inventory entries

    [Header("Inventory Data")]
    public List<CraftableItemType> items = new();
    public int capacity = 12;

    private readonly List<InventoryItemUI> currItems = new(); // Stored UI references

    private string SaveKey => $"Inventory_{inventoryType}"; // Unique key per inventory type

    private void Awake()
    {
        LoadInventory();
    }

    private void OnApplicationQuit()
    {
        SaveInventory();
    }

    public virtual bool AddItem(CraftableItemType itemInfo)
    {
        if (items.Count >= capacity)
            return false;

        items.Add(itemInfo);

        InventoryItemUI uiItem = Instantiate(inventoryItemUIPrefab, content);
        uiItem.Init(inventoryType, itemInfo);

        currItems.Add(uiItem);

        SaveInventory(); // Save on add
        return true;
    }

    public virtual bool RemoveItem(CraftableItemType itemInfo)
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

        SaveInventory(); // Save on remove
        return true;
    }

    // Save items as JSON array of names
    private void SaveInventory()
    {
        string json = JsonUtility.ToJson(new InventorySaveData { itemNames = items.Select(i => i.ToString()).ToArray() });
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
    }

    // Load items from PlayerPrefs
    private void LoadInventory()
    {
        if (!PlayerPrefs.HasKey(SaveKey)) return;

        string json = PlayerPrefs.GetString(SaveKey);
        InventorySaveData data = JsonUtility.FromJson<InventorySaveData>(json);

        foreach (var itemName in data.itemNames)
        {
            Enum.TryParse(itemName, true, out CraftableItemType craftableItemType);
            AddItem(craftableItemType); // Will also create UI
        }
    }

    [System.Serializable]
    private class InventorySaveData
    {
        public string[] itemNames;
    }
}

public enum InventoryType
{
    Player,
    Storage
}