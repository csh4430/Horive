using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour
{
    private Image image = null;
    private Text text = null;

    private void Awake()
    {
        image = GetComponent<Image>();
        text = GetComponent<Text>();
    }
    private void OnEnable()
    {
        Show();
    }

    private void Show()
    {
        image?.DOFade(1, 1f).OnComplete(() => { image?.DOFade(0, 2f).SetDelay(2); });
        text?.DOFade(1, 1f).OnComplete(() => { text?.DOFade(0, 2f).SetDelay(2); });
    }

    public void StopTween()
    {
        if (image != null)
            DOTween.Kill(image);
        if(text != null)
            DOTween.Kill(text);
    }
}
