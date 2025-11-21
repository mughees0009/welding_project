using UnityEngine;

public class WeldingManager : MonoBehaviour
{
    private static WeldingManager instance;
    public static WeldingManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<WeldingManager>();
            }
            return instance;
        }
    }

    public Transform repairableItemPoint;
    public float requiredProgress = 1f;


    private RepairableItem currentItem;
    private float weldProgress;


    public void SetRepairableItem(CraftableItemData data)
    {
        foreach (Transform child in repairableItemPoint)
            Destroy(child.gameObject);

        var repairableItem = Instantiate(data.repairableItemPrefab, repairableItemPoint);

        currentItem = repairableItem;
        currentItem.data = data;
        weldProgress = 0;
    }

    public void AddWeldProgress(float amount)
    {
        if (currentItem == null) return;

        weldProgress += amount;

        if (weldProgress >= requiredProgress)
            CompleteRepair();
    }
    public void CheckCompletion(WeldPointGroup group)
    {
        float p = group.GetProgress01();

        if (p >= 1f)
        {
            Debug.Log("Plate fully welded!");


        }
    }
    private void CompleteRepair()
    {
        InventoryItemInfo repairedItem = new()
        {
            itemName = currentItem.requiredCraftableItemType + " Plate",
            icon = currentItem.data.repairedItemIcon,
        };

        SecondaryStorage.Instance.AddItem(repairedItem);

        Destroy(currentItem.gameObject);
    }
}
