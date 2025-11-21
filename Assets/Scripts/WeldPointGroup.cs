using UnityEngine;
using System.Collections.Generic;
using System;

public class WeldPointGroup : MonoBehaviour
{
    [Header("Corner points (in local space order)")]
    public List<Transform> corners = new();

    [Header("Weld Point Settings")]
    public float pointSpacing = 0.05f;
    public float pointRadius = 0.04f;

    [Header("Weld Patch Prefab")]
    public WelderPatch patchPrefab;

    public List<WeldPoint> weldPoints = new();

    private void Awake()
    {
        GenerateWeldPoints();
        SpawnWeldPatches();
    }

    [ContextMenu(nameof(GenerateWeldPoints))]
    public void GenerateWeldPoints()
    {
        weldPoints.Clear();
        if (corners.Count < 2) return;

        // Generate points for each edge of the polygon
        for (int i = 0; i < corners.Count; i++)
        {
            Vector3 a = corners[i].localPosition;
            Vector3 b = corners[(i + 1) % corners.Count].localPosition;

            GeneratePointsOnEdge(a, b);
        }
    }

    private void GeneratePointsOnEdge(Vector3 a, Vector3 b)
    {
        float length = Vector3.Distance(a, b);
        int count = Mathf.CeilToInt(length / pointSpacing);

        if (count < 2) return;

        Vector3 edgeDir = (b - a).normalized;
        Quaternion edgeRot = Quaternion.FromToRotation(Vector3.right, edgeDir);

        // Skip first and last point to avoid duplicates at corners
        for (int i = 1; i < count; i++)
        {
            float t = (float)i / count;
            Vector3 p = Vector3.Lerp(a, b, t);
            weldPoints.Add(new WeldPoint(p, pointRadius, edgeRot));
        }
    }

    private void SpawnWeldPatches()
    {
        if (patchPrefab == null)
        {
            Debug.LogWarning("No patch prefab assigned.");
            return;
        }

        // Instantiate patch prefabs for all weld points
        foreach (var wp in weldPoints)
        {
            Vector3 worldPos = transform.TransformPoint(wp.localPos);
            var patch = Instantiate(patchPrefab, worldPos, transform.rotation * wp.localRot, transform);
            patch.Init(wp, this);
        }
    }

    // Returns progress as a value between 0 and 1
    public float GetProgress01()
    {
        if (weldPoints.Count == 0) return 0;

        int welded = 0;
        foreach (var wp in weldPoints)
            if (wp.welded) welded++;

        return (float)welded / weldPoints.Count;
    }

    // Called when a single weld point is welded
    public void OnWeld()
    {
        WeldingManager.Instance.OnChangeProgress(GetProgress01());
    }
}

[System.Serializable]
public class WeldPoint
{
    public Vector3 localPos;
    public bool welded = false;
    public float radius;
    public Quaternion localRot;

    public WeldPoint(Vector3 pos, float radius, Quaternion rot)
    {
        localPos = pos;
        this.radius = radius;
        localRot = rot;
    }
}
