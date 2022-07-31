using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class UIManager : MonoSingleton<UIManager>
{
    float[] Colors = new float[5] { 100, 50, 0, -50, -100 };

    #region All
    private Canvas fadeCanvas = null;
    private Image fadeImage = null;
    #endregion
    #region InGame
    private GameObject toolTips = null;
    private Text correctCntText = null;
    private GameObject inputKey;
    private List<Text> toolTextList = new List<Text>();
    private Text resultTxt = null;
    #endregion
    private GameObject settingCanvas = null;
    private List<GameObject> keySettingObjects = new List<GameObject>();
    private List<Text> keyTextList = new List<Text>();
    private List<Button> keyButtonList = new List<Button>();
    private List<Slider> soundSliderList = new List<Slider>();
    private Slider offsetSlider = null;
    private Text offsetText = null;

    private GameObject pauseCanvas = null;
    private Button restartBtn = null;
    private Button menuBtn = null;
    private Button optionBtn = null;
    private Text songTxt = null;

    private List<MusicSelector> musicSelectors = new List<MusicSelector>();

    private Button quitBtn = null;
    private Button startBtn = null;

    private Volume _vol;
    private WhiteBalance _wb;

    Sequence sequence;

    List<GameObject> keyList = new List<GameObject>();

    protected override void Awake()
    {
        base.Awake();

        startBtn = GameObject.Find("StartBtn").GetComponent<Button>();
        startBtn.onClick.AddListener(() =>
        {
            SoundManager.Instance.InteractionSource.Play();
            NoteController.Bigger(PoolManager.Instance.Pool(GameManager.Instance.originalNote).GetComponent<Notes>(), 5, 1f, Camera.main.ScreenToWorldPoint(Input.mousePosition));
            Fade(true, () =>
            {
                SceneManager.LoadScene("Select");
                GameManager.ChangeState(GameManager.GameState.Select);
                UIManager.Instance.Fade(false, null);
            });
        });
        fadeCanvas = GameObject.Find("FadeCanvas").GetComponent<Canvas>();
        fadeImage = fadeCanvas.transform.GetChild(0).GetComponent<Image>();
        DontDestroyOnLoad(fadeCanvas.gameObject);
    }

    public override void Initialize()
    {
        base.Initialize();

        _vol = GameObject.Find("Global Volume").GetComponent<Volume>();
        _vol.profile.TryGet(out _wb);

        if (GameManager.CurrentState == GameManager.GameState.InGame)
        {
            toolTextList.Clear();
            for (int i = 0; i < 4; i++)
            {
                toolTextList.Add(GameObject.Find(((Direction)i).ToString() + "ToolText").GetComponent<Text>());
            }
            toolTips = GameObject.Find("ToolTips");
            toolTips.SetActive(false);
            correctCntText = GameObject.Find("CntText").GetComponent<Text>();
            inputKey = (GameObject)Resources.Load("Prefab/InputKey");
            resultTxt = GameObject.Find("ResultText").GetComponent<Text>();
        }

        

        if (GameManager.CurrentState != GameManager.GameState.Init)
        {
            
            if(settingCanvas == null)
            {
                settingCanvas = PoolManager.Instance.Pool((GameObject)Resources.Load("Prefab/SettingCanvas"));
                DontDestroyOnLoad(settingCanvas);
                quitBtn = GameObject.Find("QuitBtn").GetComponent<Button>();
                quitBtn.onClick.AddListener(() =>
                {
                    NoteController.Bigger(PoolManager.Instance.Pool(GameManager.Instance.originalNote).GetComponent<Notes>(), 5, 1f, Camera.main.ScreenToWorldPoint(Input.mousePosition));
                    SoundManager.Instance.InteractionSource.Play();
                    GameManager.Instance.PauseGame(false);
                    Time.timeScale = 1f;
                    DOTween.timeScale = 1f;
                    SetPanel("Setting");
                    Fade(true, () => Application.Quit());
                });
                for (int i = 0; i < 4; i++)
                {
                    keySettingObjects.Add(GameObject.Find(((Direction)i).ToString() + "KeySetting"));
                    keyTextList.Add(keySettingObjects[i].transform.GetChild(1).GetChild(0).GetComponent<Text>());
                    keyButtonList.Add(keySettingObjects[i].transform.GetChild(1).GetComponent<Button>());
                }

                soundSliderList.Add(GameObject.Find("Master").GetComponent<Slider>());
                soundSliderList.Add(GameObject.Find("Music").GetComponent<Slider>());
                soundSliderList.Add(GameObject.Find("Interaction").GetComponent<Slider>());

                offsetSlider = GameObject.Find("OffsetSlider").GetComponent<Slider>();
                offsetText = GameObject.Find("OffsetText").GetComponent<Text>();

                offsetSlider.onValueChanged.AddListener((x) =>
                {
                    GameManager.Instance.Offset = x;
                    Setting load = FileManager.Instance.LoadJsonFile<Setting>(Application.streamingAssetsPath + "/Save", "Setting");
                    FileManager.Instance.SaveJson(Application.streamingAssetsPath + "/Save", "Setting", new Setting(load.keySetting, x, load.audioSetting));
                    offsetText.text = string.Format("{0:0}ms", x * 1000);
                });
            }
            settingCanvas.GetComponent<Canvas>().worldCamera = Camera.main;
            fadeCanvas.worldCamera = Camera.main;
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
        if (GameManager.CurrentState == GameManager.GameState.Select)
        {
            MusicSelector originalMusic = FindObjectOfType<MusicSelector>();
            PoolManager.Instance.poolList.Clear();
            PoolManager.Instance.DeSpawn(originalMusic.gameObject);
            musicSelectors.Clear();
            for (int i = 0; i < 4; i++)
            {
                musicSelectors.Add(PoolManager.Instance.Pool(originalMusic.gameObject, originalMusic.transform.parent).GetComponent<MusicSelector>());
                FileManager.Instance.Initialize();
                musicSelectors[i].WriteDesc(FileManager.Instance.patterns[i].Tag, FileManager.Instance.patterns[i].title, FileManager.Instance.patterns[i].composer, FileManager.Instance.patterns[i].HasCleared);
            }
            for (int i = 0; i < musicSelectors.Count; i++)
            {
                int num = i;
                musicSelectors[num].MusicSelectButton.onClick.AddListener(() =>
                {
                    SoundManager.Instance.InteractionSource.Play();
                    GameManager.Instance.Tag = musicSelectors[num].Tag;
                    NoteController.Bigger(PoolManager.Instance.Pool(GameManager.Instance.originalNote).GetComponent<Notes>(), 5, 1f, Camera.main.ScreenToWorldPoint(Input.mousePosition));
                    UIManager.Instance.Fade(true, () =>
                    {
                        SceneManager.LoadScene("Main");
                        GameManager.ChangeState(GameManager.GameState.InGame);
                        UIManager.Instance.Fade(false, null);
                    });

                });
            }
        }
        if (GameManager.CurrentState == GameManager.GameState.InGame)
        {
            pauseCanvas = GameObject.Find("PauseCanvas");
            restartBtn = pauseCanvas.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Button>();
            menuBtn = pauseCanvas.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Button>();
            optionBtn = pauseCanvas.transform.GetChild(0).GetChild(0).GetChild(2).GetComponent<Button>();
            songTxt = pauseCanvas.transform.GetChild(0).GetChild(1).GetComponent<Text>();
            restartBtn.onClick.AddListener(() =>
            {
                ResetColor();
                SoundManager.Instance.InteractionSource.Play();
                NoteController.Bigger(PoolManager.Instance.Pool(GameManager.Instance.originalNote).GetComponent<Notes>(), 5, 1f, Camera.main.ScreenToWorldPoint(Input.mousePosition));
                GameManager.Instance.PauseGame(true);

                SetPanel("Pause");
                CancelFade();
                Fade(true, () =>
                {
                    
                    foreach (var a in FindObjectsOfType<ToolTip>())
                    {
                        a.StopTween();
                    }
                    foreach (var a in FindObjectsOfType<Notes>())
                    {
                        DOTween.Kill(a.transform);
                    }
                    foreach (var a in GameObject.FindGameObjectsWithTag("Input"))
                    {
                        DOTween.Kill(a.transform);
                        DOTween.Kill(a.GetComponent<SpriteRenderer>());
                    }
                    GameManager.Instance.PauseGame(false);
                    GameManager.Instance.StopAllCoroutines();
                    StopAllCoroutines();
                    SoundManager.Instance.StopMusic();
                    SceneManager.LoadScene("Main");
                    Fade(false, null);
                });
            });
            menuBtn.onClick.AddListener(() =>
            {
                NoteController.Bigger(PoolManager.Instance.Pool(GameManager.Instance.originalNote).GetComponent<Notes>(), 5, 1f, Camera.main.ScreenToWorldPoint(Input.mousePosition));

                SoundManager.Instance.InteractionSource.Play();
                CancelFade();
                GameManager.Instance.PauseGame(true);
                SetPanel("Pause");
                Fade(true, () =>
                {
                    
                    foreach (var a in FindObjectsOfType<ToolTip>())
                    {
                        a.StopTween();
                    }
                    foreach (var a in FindObjectsOfType<Notes>())
                    {
                        DOTween.Kill(a.transform);
                    }
                    foreach (var a in GameObject.FindGameObjectsWithTag("Input"))
                    {
                        DOTween.Kill(a.transform);
                        DOTween.Kill(a.GetComponent<SpriteRenderer>());
                    }
                    GameManager.Instance.PauseGame(false);
                    GameManager.Instance.StopAllCoroutines();
                    StopAllCoroutines();
                    SoundManager.Instance.StopMusic();
                    GameManager.ChangeState(GameManager.GameState.Select);
                    SceneManager.LoadScene("Select");
                    Fade(false, null);
                });
            });
            optionBtn.onClick.AddListener(() => 
            {
                SoundManager.Instance.InteractionSource.Play();

                SetPanel("Pause");
                SetPanel("Setting");
            });
            pauseCanvas.SetActive(false);
        }
    }

    public IEnumerator ShowToolTips()
    {
        toolTips.SetActive(true);
        yield return new WaitForSeconds(5f);
        toolTips.SetActive(false);
    }

    public void SetLifeText(int life)
    {
        if (life < 0) return;
        correctCntText.text = string.Format("Life : {0}", life);
    }

    public void SetOffsetUI(float offset)
    {
        offsetSlider.value = offset;
        offsetText.text = string.Format("{0:0}ms", offset * 1000);
    }

    public void ShowKeyInput(Direction key)
    {
        sequence = DOTween.Sequence().OnStart(() =>
        {
            GameObject keyObject = PoolManager.Instance.Pool(inputKey);
            keyList.Add(keyObject);
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
                keyList.Remove(keyObject);
            };
        });
    }

    public void StopTween()
    {
        foreach(var key in keyList)
        {
            DOTween.Kill(key.transform);
            DOTween.Kill(key.GetComponent<SpriteRenderer>());
        }
        keyList.Clear();
    }

    public void SetPanel(string panel)
    {
        if (GameManager.CurrentState == GameManager.GameState.Init) return;
        switch (panel)
        {
            case "Pause":
                if (settingCanvas.activeInHierarchy)
                {
                    SetPanel("Setting");
                    SetPanel("Pause");
                    return;
                }
                pauseCanvas.SetActive(!pauseCanvas.activeInHierarchy);
                GameManager.Instance.PauseGame(pauseCanvas.activeInHierarchy);
                if (pauseCanvas.activeInHierarchy)
                {
                    SoundManager.Instance.MusicSource.Pause();
                    songTxt.text = "Now Playing : " + FileManager.Instance.pattern.title;
                }
                else
                    SoundManager.Instance.MusicSource.Play();
                Time.timeScale = pauseCanvas.activeInHierarchy ? 0 : 1;
                DOTween.timeScale = pauseCanvas.activeInHierarchy ? 0 : 1;
                break;
            case "Setting":
                settingCanvas.SetActive(!settingCanvas.activeInHierarchy);
                GameManager.Instance.PauseGame(settingCanvas.activeInHierarchy);
                if(settingCanvas.activeInHierarchy)
                    SoundManager.Instance.MusicSource.Pause();
                else
                    SoundManager.Instance.MusicSource.Play();
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
            case "Interaction":
                soundSliderList[2].value = value;
                break;
        }
    }

    public void SetKey(Direction dir, KeyCode key){
        keyTextList[(int)dir].text = key.ToString();
        if(GameManager.CurrentState == GameManager.GameState.InGame)
            toolTextList[(int)dir].text = key.ToString();
    }

    public void Fade(bool isIn, Action onComplete, float delay = 0)
    {
        ResetColor();
        switch (isIn)
        {
            case true:
                fadeImage.raycastTarget = true;
                fadeImage.maskable = true;
                fadeImage.DOFade(1, 1).OnComplete(() =>
                {
                    onComplete?.Invoke();
                }).SetDelay(delay);
                break;

            case false:
                fadeImage.DOFade(0, 1).OnComplete(() => 
                {
                    onComplete?.Invoke();
                    fadeImage.raycastTarget = false; fadeImage.maskable = false;
                }).SetDelay(delay);
                break;

        }
    }

    public void CancelFade()
    {
        DOTween.Kill(fadeImage);
    }

    public void ShowResult(string result)
    {
        resultTxt.text = result;
        resultTxt.DOFade(1, 1f);
        ResetColor();
        Fade(true, () =>
        {
            StopTween();
            GameManager.ChangeState(GameManager.GameState.Select);
            SceneManager.LoadScene("Select");
            Fade(false, null);
        }, 1f);

        StartCoroutine(SoundDelay());
    }
    
    private IEnumerator SoundDelay()
    {

        yield return new WaitForSeconds(0.5f);
        SoundManager.Instance.StopMusic();
    }

    public void ChangeScreenWhenFail(int life)
    {
        _wb.temperature.value = 100f;
        DOTween.To(() => _wb.temperature.value, x => _wb.temperature.value = x, 100, 0.1f).OnComplete(() =>
        {
            DOTween.To(() => _wb.temperature.value, x => _wb.temperature.value = x, Colors[life], 0.2f);
        });
    }

    public void ResetColor() 
    {
        _wb.temperature.value = -100;
    }
}
