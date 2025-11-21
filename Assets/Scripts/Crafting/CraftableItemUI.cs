using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftableItemUI : MonoBehaviour
{
    public CraftableItemData craftableItemData;
    public Button button;
    public GameObject selected;

    private void Awake()
    {
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        CraftingManager.Instance.OnSelectCraftingItem(this);
    }
}
