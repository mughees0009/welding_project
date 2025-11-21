using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class WeldingManager : MonoBehaviour
{
    private static WeldingManager instance;
    public static WeldingManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindAnyObjectByType<WeldingManager>();
            return instance;
        }
    }

    [Header("Progress Settings")]
    public float requiredProgress = 1f;

    public ProgressBar progressBar;
    public Button addToStorageBtn;
    public CinemachineVirtualCamera craftingCamera;

    private RepairableItem currRepairableItem;

    private void Awake()
    {
        addToStorageBtn.onClick.AddListener(AddtoStorage);
        addToStorageBtn.gameObject.SetActive(false);
        progressBar.gameObject.SetActive(false);
        craftingCamera.gameObject.SetActive(false);
    }

    // Assign the current repairable item and enable welding UI
    public void SetRepairableItem(RepairableItem repairableItem)
    {
        currRepairableItem = repairableItem;
        progressBar.BarValue = 0;
        progressBar.gameObject.SetActive(true);
        craftingCamera.gameObject.SetActive(true);
        WelderTool.toolEnabled = true;
    }

    // Update welding progress
    public void OnChangeProgress(float weldProgress)
    {
        if (currRepairableItem == null) return;

        if (weldProgress >= 1)
        {
            currRepairableItem.isRepaired = true;
            addToStorageBtn.gameObject.SetActive(true);
        }

        progressBar.BarValue = Mathf.Round(weldProgress * 100);
    }

    // Add repaired item to storage and reset UI
    private void AddtoStorage()
    {
        addToStorageBtn.gameObject.SetActive(false);

        SecondaryStorage.Instance.AddItem(currRepairableItem.data.itemType);

        Destroy(currRepairableItem.gameObject);

        progressBar.gameObject.SetActive(false);
        craftingCamera.gameObject.SetActive(false);
        WelderTool.toolEnabled = false;
    }
}
