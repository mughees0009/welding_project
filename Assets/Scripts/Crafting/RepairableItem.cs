using System.Collections;
using DG.Tweening;
using UnityEngine;

public class RepairableItem : MonoBehaviour
{
    [Header("Animation Settings")]
    public float duration = 2;
    public int numJumps = 1;
    public float jumpPower = 1;
    public float delay = 1;

    [Header("Item References")]
    public CraftableItemData data;
    private CraftableItem craftableItem;
    public Transform crafableItemPoint;      // Target point for jump animation
    public Transform crafableItemSpawnPoint; // Initial spawn position

    [HideInInspector] public bool isRepaired;

    // Initialize the repairable item with its data and spawn it
    public void Init(CraftableItemData data)
    {
        this.data = data;
        SpawnItem();
    }

    [ContextMenu(nameof(SpawnItem))]
    private void SpawnItem()
    {
        StartCoroutine(SpawnItem_CO());
    }

    // Coroutine to handle spawn and jump animation of the item
    private IEnumerator SpawnItem_CO()
    {
        // Instantiate item if not already present
        craftableItem ??= Instantiate(
            data.craftableItemPrefab,
            crafableItemSpawnPoint.position,
            crafableItemSpawnPoint.rotation,
            crafableItemSpawnPoint
        );

        // Animate scale from zero to full size
        craftableItem.transform.localScale = Vector3.zero;
        yield return new WaitForSeconds(1);
        craftableItem.transform.DOScale(1, 0.4f);

        // Wait before jumping to target point
        yield return new WaitForSeconds(delay);
        craftableItem.transform.DOJump(crafableItemPoint.position, jumpPower, numJumps, duration);
    }
}
