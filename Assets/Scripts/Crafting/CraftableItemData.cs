using UnityEngine;

[CreateAssetMenu(menuName = "Crafting/CraftableItem")]
public class CraftableItemData : ScriptableObject
{
    public CraftableItemType itemType;

    public string itemName;

    public CraftableItem craftableItemPrefab;   // Prefab used when crafting
    public RepairableItem repairableItemPrefab; // Prefab used after repairing

    public Sprite craftableItemIcon; // Icon for crafting UI
    public Sprite repairedItemIcon;  // Icon after repair
}
