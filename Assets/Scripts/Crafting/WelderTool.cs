using DG.Tweening;
using UnityEngine;

public class WelderTool : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSmoothness = 15f;
    public float planeHeight = 0.5f;

    [Header("Tool References")]
    public Transform normalPoint;
    public Transform weldingPoint;
    public Transform toolMesh;
    public GameObject weldVfx;

    [Header("Welding")]
    public Transform weldNosel;
    public float weldDetectionRadius;
    public LayerMask patchMask;
    public AudioSource weldingSound;

    private Camera cam;
    private bool isDragging;

    public static bool toolEnabled;

    private void Start()
    {
        cam = Camera.main;
        OnStopWeld();

        // Configure audio
        weldingSound.playOnAwake = false;
        weldingSound.loop = true;
    }

    private void Update()
    {
        if (!toolEnabled)
        {
            OnStopWeld();
            return;
        }

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
            DetectPatch();
        }
    }

    // Move tool back to resting position
    private void OnStopWeld()
    {
        toolMesh.DOKill(false);
        toolMesh.SetParent(normalPoint, true);
        toolMesh.DOLocalMove(Vector3.zero, 0.2f);
        toolMesh.DOLocalRotate(Vector3.zero, 0.2f);
        weldVfx.SetActive(false);
        weldingSound.Stop();
    }

    // Move tool to welding position
    private void OnStartWeld()
    {
        toolMesh.DOKill(false);
        toolMesh.SetParent(weldingPoint, true);
        toolMesh.DOLocalMove(Vector3.zero, 0.2f);
        toolMesh.DOLocalRotate(Vector3.zero, 0.2f);
        weldVfx.SetActive(true);
    }

    // Smoothly move tool with mouse on a horizontal plane
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

    // Detect weldable patches and trigger welding
    private void DetectPatch()
    {
        Collider[] cols = Physics.OverlapSphere(weldNosel.position, weldDetectionRadius, patchMask);

        // Stop sound if no patches detected
        if (cols.Length == 0 && weldingSound.isPlaying)
        {
            weldingSound.Stop();
        }

        foreach (var col in cols)
        {
            if (!col.TryGetComponent<WelderPatch>(out var patch))
                continue;

            patch.TryWeld(weldNosel.position);

            if (!weldingSound.isPlaying)
                weldingSound.Play();
        }
    }

    // Draw gizmo for weld detection radius
    private void OnDrawGizmos()
    {
        if (weldNosel == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(weldNosel.position, weldDetectionRadius);
    }
}
