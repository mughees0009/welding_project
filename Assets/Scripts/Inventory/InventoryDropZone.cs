using UnityEngine;

public class InventoryDropZone : MonoBehaviour
{
    public InventoryType zoneType;

    // Called by UI item when dropped on this zone
    public void OnItemDropped(InventoryItemUI itemUI)
    {
        if (itemUI.currInventory == zoneType)
            return; // Already in this inventory

        switch (zoneType)
        {
            case InventoryType.Player:
                if (PlayerInventory.Instance.AddItem(itemUI.itemInfo))
                    SecondaryStorage.Instance.RemoveItem(itemUI.itemInfo);
                break;

            case InventoryType.Storage:
                if (SecondaryStorage.Instance.AddItem(itemUI.itemInfo))
                    PlayerInventory.Instance.RemoveItem(itemUI.itemInfo);
                break;
        }
    }
}
