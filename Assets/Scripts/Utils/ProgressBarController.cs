using UnityEngine;
using UnityEngine.UI;

public class ProgressBarController : MonoBehaviour
{
    [SerializeField] private GameObject root;
    [SerializeField] private Image fill;

    public void Show() { root.SetActive(true); fill.fillAmount = 0f; }
    public void Hide() { root.SetActive(false); }
    public void SetProgress(float v) => fill.fillAmount = Mathf.Clamp01(v);
}
