using System.Net;
using DG.Tweening;
using UnityEngine;

public class WelderTool : MonoBehaviour
{
    public float moveSmoothness = 15f;
    public float planeHeight = 0.5f;

    public Transform normalPoint;
    public Transform weldingPoint;
    public Transform toolMesh;
    public GameObject weldVfx;

    [Header("Welding")]
    public Transform weldNosel;
    public float weldDetectionRadius;
    public LayerMask edgeMask;
    public ParticleSystem sparkVfx;

    private Camera cam;
    private bool isDragging;

    private void Start()
    {
        cam = Camera.main;
        OnStopWeld();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            OnStartWeld();
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;

            OnStopWeld();
        }

        if (isDragging)
        {
            MoveToolWithMouse();
            WeldCheck();
        }
    }

    private void OnStopWeld()
    {
        toolMesh.DOKill(false);
        toolMesh.SetParent(normalPoint, true);
        toolMesh.DOLocalMove(Vector3.zero, 0.2f);
        toolMesh.DOLocalRotate(Vector3.zero, 0.2f);
        weldVfx.SetActive(false);
    }

    private void OnStartWeld()
    {
        toolMesh.DOKill(false);
        toolMesh.SetParent(weldingPoint, true);
        toolMesh.DOLocalMove(Vector3.zero, 0.2f);
        toolMesh.DOLocalRotate(Vector3.zero, 0.2f);
        weldVfx.SetActive(true);
    }

    private void MoveToolWithMouse()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        Plane movementPlane = new Plane(Vector3.up, new Vector3(0, planeHeight, 0));

        if (movementPlane.Raycast(ray, out float enter))
        {
            Vector3 hitPos = ray.GetPoint(enter);
            hitPos.y = planeHeight;

            transform.position = Vector3.Lerp(transform.position, hitPos, Time.deltaTime * moveSmoothness);
        }
    }

    private void WeldCheck()
    {
        Collider[] cols = Physics.OverlapSphere(weldNosel.position, weldDetectionRadius, edgeMask);

        foreach (var col in cols)
        {
            if (!col.TryGetComponent<WeldPointGroup>(out var group))
                continue;

            foreach (var wp in group.weldPoints)
            {
                if (wp.welded) continue;

                Vector3 worldPos = group.transform.TransformPoint(wp.localPos);
                float dist = Vector3.Distance(weldNosel.position, worldPos);

                if (dist <= weldDetectionRadius)
                {
                    wp.welded = true;
                    Instantiate(sparkVfx, worldPos, Quaternion.Euler(-90, 0, 0));
                }
            }

            WeldingManager.Instance.CheckCompletion(group);
        }
    }

    private void OnDrawGizmos()
    {
        if (weldNosel == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(weldNosel.position, weldDetectionRadius);
    }

}
