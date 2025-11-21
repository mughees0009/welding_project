using UnityEngine;

[CreateAssetMenu(menuName = "Crafting/CraftableItem")]
public class CraftableItemData : ScriptableObject
{
    public CraftableItemType itemType;

    public string itemName;

    public CraftableItem craftableItemPrefab;

    public RepairableItem repairableItemPrefab;

    public Sprite craftableItemIcon;
    public Sprite repairedItemIcon;
}
