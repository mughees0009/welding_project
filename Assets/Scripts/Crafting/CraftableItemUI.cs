using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftableItemUI : MonoBehaviour
{
    public Image image;
    public CraftableItemData craftableItemData;
    public Button button;
    public GameObject selected;

    // Initialize the UI with the given craftable item data
    internal void Init(CraftableItemData craftableItemData)
    {
        this.craftableItemData = craftableItemData;
        button.onClick.AddListener(OnClick);
        image.sprite = craftableItemData.craftableItemIcon;
    }

    // Called when the UI button is clicked
    private void OnClick()
    {
        CraftingManager.Instance.OnSelectCraftingItem(this);
    }
}
