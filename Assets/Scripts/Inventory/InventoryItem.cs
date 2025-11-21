using System;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    public InventoryItemInfo inventoryItemInfo;
}

[Serializable]
public class InventoryItemInfo
{
    public string itemName;
    public Sprite icon;
}
