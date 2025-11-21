using System;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemUI : MonoBehaviour
{
    public InventoryType currInventory;
    public InventoryItemInfo itemInfo;
    public Image itemImage;
    public Button button;


    public void Init(Sprite sprite, InventoryType inventoryType, InventoryItemInfo itemInfo)
    {
        itemImage.sprite = sprite;
        currInventory = inventoryType;
        this.itemInfo = itemInfo;
        button.onClick.AddListener(OnClick);
    }


    private void OnClick()
    {
        switch (currInventory)
        {
            case InventoryType.Player:
                if (SecondaryStorage.Instance.AddItem(itemInfo))
                    PlayerInventory.Instance.RemoveItem(itemInfo);
                break;

            case InventoryType.Storage:
                if (PlayerInventory.Instance.AddItem(itemInfo))
                    SecondaryStorage.Instance.RemoveItem(itemInfo);
                break;
        }
    }

}
