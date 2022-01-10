using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour
{
    private Image image = null;

    private void Awake()
    {
        image = GetComponent<Image>();
    }
    private void OnEnable()
    {
        Show();
    }

    private void Show()
    {
        image.DOFade(1, 1f).OnComplete(() => { image.DOFade(0, 2f).SetDelay(2); });
    }
}
