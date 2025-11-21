using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Linq;

public class InventoryItemUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public InventoryType currInventory;
    public CraftableItemType itemInfo;
    public Image itemImage;

    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;

    private Transform originalParent;
    private int originalSiblingIndex;
    private LayoutElement layoutElement;

    private bool droppedOnZone = false;


    public void Init(InventoryType inventoryType, CraftableItemType itemInfo)
    {
        CraftableItemData craftableItemData = CraftingManager.Instance.craftableItemDatas.FirstOrDefault(x => x.itemType == itemInfo);

        this.currInventory = inventoryType;
        this.itemInfo = itemInfo;
        itemImage.sprite = craftableItemData.repairedItemIcon;

        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        canvas = GetComponentInParent<Canvas>();

        layoutElement = GetComponent<LayoutElement>();
        if (layoutElement == null)
            layoutElement = gameObject.AddComponent<LayoutElement>();
    }

    // -----------------------------------------------------------
    // DRAG START
    // -----------------------------------------------------------
    public void OnBeginDrag(PointerEventData eventData)
    {
        droppedOnZone = false;

        originalParent = transform.parent;
        originalSiblingIndex = transform.GetSiblingIndex();

        layoutElement.ignoreLayout = true;

        // Move to canvas so it's above all UI
        transform.SetParent(canvas.transform, true);
        transform.SetAsLastSibling();

        canvasGroup.blocksRaycasts = false;  // MOST IMPORTANT FIX
        canvasGroup.alpha = 0.7f;
    }

    // -----------------------------------------------------------
    // DRAGGING
    // -----------------------------------------------------------
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = eventData.position;
    }

    // -----------------------------------------------------------
    // DRAG END
    // -----------------------------------------------------------
    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        GameObject hit = eventData.pointerCurrentRaycast.gameObject;
        InventoryDropZone dropZone = null;

        if (hit != null)
            dropZone = hit.GetComponentInParent<InventoryDropZone>();

        if (dropZone != null)
        {
            droppedOnZone = true;
            dropZone.OnItemDropped(this);
        }

        // If no valid drop zone → return back
        if (!droppedOnZone)
        {
            transform.SetParent(originalParent, false);
            transform.SetSiblingIndex(originalSiblingIndex);
        }

        layoutElement.ignoreLayout = false;
    }


}
