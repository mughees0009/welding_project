using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    // Singleton instance
    private static CraftingManager instance;
    public static CraftingManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindAnyObjectByType<CraftingManager>();
            return instance;
        }
    }

    [Header("UI References")]
    public CraftableItemUI craftableItemUIPrefab;
    public Transform craftableItemUIParent;
    public CraftableItemData[] craftableItemDatas;

    [Header("Crafting Points")]
    public Transform repairableItemPoint;

    private CraftableItemUI selectedCraftableUiItem;

    private void Start()
    {
        // Instantiate UI for each craftable item
        foreach (var craftableItemData in craftableItemDatas)
        {
            CraftableItemUI craftableItemUI = Instantiate(craftableItemUIPrefab, craftableItemUIParent);
            craftableItemUI.Init(craftableItemData);

            if (selectedCraftableUiItem == null)
            {
                OnSelectCraftingItem(craftableItemUI);
            }
        }
    }

    // Called when a craftable item is selected from UI
    public void OnSelectCraftingItem(CraftableItemUI craftableItemUI)
    {
        if (selectedCraftableUiItem != null)
            selectedCraftableUiItem.selected.SetActive(false);

        selectedCraftableUiItem = craftableItemUI;
        selectedCraftableUiItem.selected.SetActive(true);
    }

    // Start crafting the selected item
    public void StartCrafting()
    {
        if (selectedCraftableUiItem == null)
            return;

        CraftableItemData data = selectedCraftableUiItem.craftableItemData;

        // Clear previous repairable items
        foreach (Transform child in repairableItemPoint)
            Destroy(child.gameObject);

        // Instantiate new repairable item
        var currRepairableItem = Instantiate(data.repairableItemPrefab, repairableItemPoint);
        currRepairableItem.Init(data);

        // Set in WeldingManager for welding workflow
        WeldingManager.Instance.SetRepairableItem(currRepairableItem);
    }
}

public enum CraftableItemType
{
    Square,
    Triangle,
    Pentagon
}
