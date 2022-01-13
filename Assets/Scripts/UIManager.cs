using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using UnityEngine.SceneManagement;

public class UIManager : MonoSingleton<UIManager>
{
    #region All
    private GameObject fadeCanvas = null;
    private Image fadeImage = null;
    #endregion
    #region InGame
    private GameObject toolTips = null;
    private Text correctCntText = null;
    private GameObject inputKey;
    private List<Text> toolTextList = new List<Text>();
    #endregion
    private GameObject settingCanvas = null;
    private List<GameObject> keySettingObjects = new List<GameObject>();
    private List<Text> keyTextList = new List<Text>();
    private List<Button> keyButtonList = new List<Button>();
    private List<Slider> soundSliderList = new List<Slider>();

    private MusicSelector[] musicSelectors = null;

    protected override void Awake()
    {
        base.Awake();

        fadeCanvas = GameObject.Find("FadeCanvas");
        fadeImage = fadeCanvas.transform.GetChild(0).GetComponent<Image>();
        DontDestroyOnLoad(fadeCanvas);
    }

    public override void Initialize()
    {
        base.Initialize();

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

        if(GameManager.CurrentState == GameManager.GameState.Select)
        {
            musicSelectors = FindObjectsOfType<MusicSelector>();
            musicSelectors[0].MusicSelectButton.onClick.AddListener(() =>
            {
                NoteController.Bigger(PoolManager.Instance.Pool(GameManager.Instance.originalNote).GetComponent<Notes>(), 5, 1f, Camera.main.ScreenToWorldPoint(Input.mousePosition));
                UIManager.Instance.Fade(true, () =>
                {
                    GameManager.ChangeState(GameManager.GameState.InGame);
                    SceneManager.LoadScene("Main");
                    UIManager.Instance.Fade(false, null);
                });

            });
        }

        if (GameManager.CurrentState != GameManager.GameState.Init)
        {
            if(settingCanvas == null)
            {
                settingCanvas = PoolManager.Instance.Pool((GameObject)Resources.Load("Prefab/SettingCanvas"));
                DontDestroyOnLoad(settingCanvas);
                for (int i = 0; i < 4; i++)
                {
                    keySettingObjects.Add(GameObject.Find(((Direction)i).ToString() + "KeySetting"));
                    keyTextList.Add(keySettingObjects[i].transform.GetChild(1).GetChild(0).GetComponent<Text>());
                    keyButtonList.Add(keySettingObjects[i].transform.GetChild(1).GetComponent<Button>());
                }

                soundSliderList.Add(GameObject.Find("Master").GetComponent<Slider>());
                soundSliderList.Add(GameObject.Find("Music").GetComponent<Slider>());
                soundSliderList.Add(GameObject.Find("HitSound").GetComponent<Slider>());
                soundSliderList.Add(GameObject.Find("Interaction").GetComponent<Slider>());
            }
            settingCanvas.GetComponent<Canvas>().worldCamera = Camera.main;

            soundSliderList[0].onValueChanged.AddListener((value) =>
            {
                SoundManager.Instance.AudioControl("Master", value);
            });
            soundSliderList[1].onValueChanged.AddListener((value) =>
            {
                SoundManager.Instance.AudioControl("Music", value);
            });
            soundSliderList[2].onValueChanged.AddListener((value) =>
            {
                SoundManager.Instance.AudioControl("HitSound", value);
            });
            soundSliderList[3].onValueChanged.AddListener((value) =>
            {
                SoundManager.Instance.AudioControl("Interaction", value);
            });
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

    public void SetSlider(string key, float value)
    {
        switch (key)
        {
            case "Master":
                soundSliderList[0].value = value;
                break;
            case "Music":
                soundSliderList[1].value = value;
                break;
            case "HitSound":
                soundSliderList[2].value = value;
                break;
            case "Interaction":
                soundSliderList[3].value = value;
                break;
        }
    }

    public void SetKey(Direction dir, KeyCode key){
        keyTextList[(int)dir].text = key.ToString();
        if(GameManager.CurrentState == GameManager.GameState.InGame)
            toolTextList[(int)dir].text = key.ToString();
    }

    public void Fade(bool isIn, Action onComplete)
    {
        switch (isIn)
        {
            case true:
                fadeImage.DOFade(1, 1).OnComplete(() => onComplete?.Invoke());
                break;

            case false:
                fadeImage.DOFade(0, 1).OnComplete(() => onComplete?.Invoke());
                break;

        }
    }

}
