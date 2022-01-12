using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoSingleton<UIManager>
{
    #region InGame
    private GameObject toolTips = null;
    private Text correctCntText = null;
    private GameObject inputKey;
    private GameObject settingCanvas = null;
    private List<Text> toolTextList = new List<Text>();
    private List<GameObject> keySettingObjects = new List<GameObject>();
    private List<Text> keyTextList = new List<Text>();
    private List<Button> keyButtonList = new List<Button>();
    #endregion

    protected override void Awake()
    {
        base.Awake();
        
    }

    public override void Initialize()
    {
        if (GameManager.CurrentState == GameManager.GameState.InGame)
        {
            for (int i = 0; i < 4; i++)
            {
                toolTextList.Add(GameObject.Find(((Direction)i).ToString() + "ToolText").GetComponent<Text>());
            }
            toolTips = GameObject.Find("ToolTips");
            toolTips.SetActive(false);
            correctCntText = GameObject.Find("CntText").GetComponent<Text>();
            inputKey = (GameObject)Resources.Load("Prefab/InputKey");
        }

        if (GameManager.CurrentState != GameManager.GameState.Init)
        {
            Debug.Log(1);
            settingCanvas = PoolManager.Instance.Pool((GameObject)Resources.Load("Prefab/SettingCanvas"));
            settingCanvas.GetComponent<Canvas>().worldCamera = Camera.main;
            DontDestroyOnLoad(settingCanvas);
            for (int i = 0; i < 4; i++)
            {
                keySettingObjects.Add(GameObject.Find(((Direction)i).ToString() + "KeySetting"));
                keyTextList.Add(keySettingObjects[i].transform.GetChild(1).GetChild(0).GetComponent<Text>());
                keyButtonList.Add(keySettingObjects[i].transform.GetChild(1).GetComponent<Button>());
            }
            settingCanvas.SetActive(false);

            for (int i = 0; i < keyButtonList.Count; i++)
            {
                int num = i;
                keyButtonList[i].onClick.AddListener(delegate ()
                {
                    Debug.Log(num);
                    Debug.Log((Direction)num);
                    InputManager.Instance.ChangeKey((Direction)num);
                });
            }
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
        GameObject keyObject = PoolManager.Instance.Pool(inputKey);
        switch (key)
        {
            case Direction.Left:
                keyObject.transform.rotation = Quaternion.Euler(0, 0, 90);
                break;
            case Direction.Right:
                keyObject.transform.rotation = Quaternion.Euler(0, 0, -90);
                break;
            case Direction.Up:
                keyObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case Direction.Down:
                keyObject.transform.rotation = Quaternion.Euler(180, 0, 0);
                break;
        }
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
                settingCanvas.SetActive(!settingCanvas.activeInHierarchy);
                GameManager.Instance.PauseGame(settingCanvas.activeInHierarchy);
                Time.timeScale = settingCanvas.activeInHierarchy ? 0 : 1;
                DOTween.timeScale = settingCanvas.activeInHierarchy ? 0 : 1;
                break;
        }
    }

    public void SetKey(Direction dir, KeyCode key){
        keyTextList[(int)dir].text = key.ToString();
        toolTextList[(int)dir].text = key.ToString();
    }
}
