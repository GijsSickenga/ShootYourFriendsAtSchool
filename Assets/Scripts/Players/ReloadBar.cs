using UnityEngine;
using System.Collections;

public class ReloadBar : MonoBehaviour {
    public GameObject reloadCanvas;
    public RectTransform reloadBar;

    public void SetEnabled(bool active)
    {
        reloadCanvas.SetActive(active);
    }

    public void UpdateReloadBar(float fireInterval, float fireRate)
    {
        reloadBar.sizeDelta = new Vector2(fireInterval / fireRate * 50, reloadBar.sizeDelta.y);
    }
}
