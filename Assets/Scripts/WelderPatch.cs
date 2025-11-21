using UnityEngine;
using DG.Tweening;
using System;

public class WelderPatch : MonoBehaviour
{
    [Header("Refs")]
    public WeldPointGroup weldGroup;
    public WeldPoint weldPoint;

    [Header("Color Animation")]
    [SerializeField] private Color hotColor = new Color(1f, 0.45f, 0f);
    [SerializeField] private Color coolColor = Color.gray;
    [SerializeField] private float coolTime = 1.2f;

    [Header("VFX")]
    public GameObject sparkVfxPrefab;
    public float sparkYOffset = 0.2f;

    public Renderer rend;

    private Tweener colorTween;
    private GameObject currentSpark;

    private void Awake()
    {
        // Ensure renderer reference and start disabled
        if (!rend) rend = GetComponentInChildren<Renderer>();
        rend.enabled = false;
    }

    // Initialize weld patch with specific point and group
    public void Init(WeldPoint wp, WeldPointGroup group)
    {
        weldPoint = wp;
        weldGroup = group;
        rend.enabled = false;
    }

    // Called when welding is attempted at a position
    public void TryWeld(Vector3 hitPos)
    {
        rend.enabled = true;

        // Mark weld point as welded and notify group
        if (!weldPoint.welded)
        {
            weldPoint.welded = true;
            weldGroup.OnWeld();
        }

        // Stop previous color tween if active
        if (colorTween != null && colorTween.IsActive())
            colorTween.Kill();

        // Animate color from hot to cool
        rend.material.color = hotColor;
        colorTween = rend.material.DOColor(coolColor, coolTime).SetEase(Ease.OutQuad);

        // Spawn spark effect once
        if (sparkVfxPrefab != null && currentSpark == null)
        {
            currentSpark = Instantiate(
                sparkVfxPrefab,
                hitPos + new Vector3(0, sparkYOffset, 0),
                Quaternion.Euler(-90, 0, 0)
            );
        }
    }
}
