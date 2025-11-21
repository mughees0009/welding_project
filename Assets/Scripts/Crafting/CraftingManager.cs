using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    private static CraftingManager instance;
    public static CraftingManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<CraftingManager>();
            }
            return instance;
        }
    }






    private CraftableItemUI selectedCraftableUiItem;

    public void OnSelectCraftingItem(CraftableItemUI craftableItemUI)
    {
        if (selectedCraftableUiItem != null)
            selectedCraftableUiItem.selected.SetActive(false);

        selectedCraftableUiItem = craftableItemUI;

        selectedCraftableUiItem.selected.SetActive(true);

    }

    public void StartCrafting()
    {
        if (selectedCraftableUiItem == null)
            return;

        CraftableItemData data = selectedCraftableUiItem.craftableItemData;

        WeldingManager.Instance.SetRepairableItem(data);
    }

}
public enum CraftableItemType
{
    Square,
    Triangle,
    Pentagone
}

