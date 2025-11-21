using UnityEngine;
using System.Collections.Generic;

public class WeldPointGroup : MonoBehaviour
{
    [Header("Corner points (in local space order)")]
    public List<Transform> corners = new List<Transform>();

    [Header("Weld Point Settings")]
    public float pointSpacing = 0.05f;
    public float pointRadius = 0.04f;

    [Header("Weld Patch Prefab (mesh patch)")]
    public GameObject weldPatchPrefab;

    public List<WeldPoint> weldPoints = new List<WeldPoint>();

    private void Awake()
    {
        GenerateWeldPoints();
        SpawnWeldPatches();
    }

    [ContextMenu(nameof(GenerateWeldPoints))]
    public void GenerateWeldPoints()
    {
        weldPoints.Clear();

        if (corners.Count < 2)
        {
            Debug.LogWarning("Need at least 2 corners for welding.");
            return;
        }

        for (int i = 0; i < corners.Count; i++)
        {
            Vector3 a = corners[i].localPosition;
            Vector3 b = corners[(i + 1) % corners.Count].localPosition; // closed shape

            GeneratePointsOnEdge(a, b);
        }
    }

    private void GeneratePointsOnEdge(Vector3 a, Vector3 b)
    {
        float length = Vector3.Distance(a, b);
        int count = Mathf.CeilToInt(length / pointSpacing);

        for (int i = 0; i <= count; i++)
        {
            float t = (float)i / count;
            Vector3 p = Vector3.Lerp(a, b, t);

            weldPoints.Add(new WeldPoint(p, pointRadius));
        }
    }

    // ----------------------------------------------------------
    // Spawn PREFAB PATCHES for each weld point (DISABLED initially)
    // ----------------------------------------------------------
    private void SpawnWeldPatches()
    {
        if (weldPatchPrefab == null)
        {
            Debug.LogWarning("No weldPatchPrefab assigned.");
            return;
        }

        foreach (var wp in weldPoints)
        {
            Vector3 worldPos = transform.TransformPoint(wp.localPos);

            GameObject patch = Instantiate(weldPatchPrefab, worldPos, transform.rotation, transform);

            patch.SetActive(false);  // hide initially

            wp.patchObj = patch;
        }
    }

    // Called when Welder script welds a point
    public void MarkWelded(WeldPoint wp)
    {
        if (!wp.welded)
        {
            wp.welded = true;

            if (wp.patchObj != null)
                wp.patchObj.SetActive(true);
        }
    }

    public float GetProgress01()
    {
        if (weldPoints == null || weldPoints.Count == 0)
            return 0f;

        int weldedCount = 0;

        foreach (var wp in weldPoints)
            if (wp.welded) weldedCount++;

        return (float)weldedCount / weldPoints.Count;
    }

    private void OnDrawGizmos()
    {
        if (weldPoints == null || weldPoints.Count == 0) return;

        Gizmos.matrix = transform.localToWorldMatrix;

        foreach (var wp in weldPoints)
        {
            Gizmos.color = wp.welded ? Color.green : Color.red;
            Gizmos.DrawSphere(wp.localPos, wp.radius);
        }

        Gizmos.color = Color.yellow;
        foreach (var c in corners)
        {
            if (c)
                Gizmos.DrawCube(c.position, Vector3.one * pointRadius);
        }
    }
}

[System.Serializable]
public class WeldPoint
{
    public Vector3 localPos;
    public bool welded;
    public float radius;

    [System.NonSerialized]
    public GameObject patchObj; // assigned at runtime

    public WeldPoint(Vector3 localPos, float radius)
    {
        this.localPos = localPos;
        this.radius = radius;
        welded = false;
    }
}
