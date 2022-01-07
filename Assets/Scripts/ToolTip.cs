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
        Twinkle();
    }

    private void Twinkle()
    {
        image.DOFade(0, 0.5f).OnComplete(() => image.DOFade(1, 0.5f)).SetDelay(1f);
        image.DOFade(0, 0.5f).OnComplete(() => image.DOFade(1, 0.5f)).SetDelay(2f);
        image.DOFade(0, 1f).SetDelay(3.5f).OnComplete(() => { image.DOFade(1, 0).SetDelay(5); });
    }
}
