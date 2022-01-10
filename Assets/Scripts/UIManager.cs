using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] private GameObject toolTips = null;
    [SerializeField] private Text correctCntText = null;
    [SerializeField] private GameObject[] inputKey = new GameObject[4];

    public IEnumerator ShowToolTips()
    {
        toolTips.SetActive(true);
        yield return new WaitForSeconds(5f);
        toolTips.SetActive(false);
    }

    public void SetScoreText(int correctCnt)
    {
        correctCntText.text = string.Format("Correct Notes : {0}", correctCnt);
    }

    public void ShowKeyInput(Direction key)
    {
        GameObject keyObject = PoolManager.Instance.Pool(inputKey[(int)key]);
        keyObject.GetComponent<SpriteRenderer>().DOFade(0, 1f);
        keyObject.transform.DOScale(Vector2.one * 20, 1f).onComplete += () =>
        {
            keyObject.GetComponent<SpriteRenderer>().color = Color.white;
            keyObject.transform.localScale = Vector3.one;
            PoolManager.Instance.DeSpawn(keyObject);
        };
    }
}
