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
    [SerializeField] private GameObject settingPanel = null;
    [SerializeField] private List<Text> keyTextList = new List<Text>();
    [SerializeField] private List<Text> toolTextList = new List<Text>();
    [SerializeField] private List<Button> keyButtonList = new List<Button>();

    public void Start()
    {
        for(int i = 0; i < keyButtonList.Count; i++)
        {
            int num =  i;
            keyButtonList[i].onClick.AddListener(delegate()
            {
                Debug.Log(num);
                Debug.Log((Direction)num);
                InputManager.Instance.ChangeKey((Direction)num);
            });
        }
    }

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

    public void SetPanel(string panel)
    {
        switch (panel)
        {
            case "Setting":
                settingPanel.SetActive(!settingPanel.activeInHierarchy);
                GameManager.Instance.hasPaused = settingPanel.activeInHierarchy;
                Time.timeScale = settingPanel.activeInHierarchy ? 0 : 1;
                DOTween.timeScale = settingPanel.activeInHierarchy ? 0 : 1;
                break;
        }
    }

    public void SetKey(Direction dir, KeyCode key){
        keyTextList[(int)dir].text = key.ToString();
        toolTextList[(int)dir].text = key.ToString();
    }
}
