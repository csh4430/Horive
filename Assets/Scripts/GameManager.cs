using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoSingleton<GameManager>
{
    public enum GameState
    {
        Init,
        Select,
        Setting,
        InGame
    }

    #region InGame
    public GameObject originalNote { get; private set; }
    private float offset = 0.5f;
    private int correctNotes = 0;
    public int Life { get; private set; }
    public bool hasPaused { get; private set; }
    public int Tag { get; set; }
    #endregion

    public static GameState CurrentState { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        CurrentState = GameState.Init;
        Application.targetFrameRate = 300;
    }

    public override void Initialize()
    {
        base.Initialize();
        if (CurrentState == GameState.InGame)
        {
            //centerObject = GameObject.Find("Center");
            Life = 5;
            UIManager.Instance.SetLifeText(Life);
        }
        hasPaused = true;
        originalNote = (GameObject)Resources.Load("Prefab/Circle");

        if (CurrentState == GameState.InGame)
        {

            StartCoroutine(Ready());
        }
    }

    void Start()
    {

    }
    void Update()
    {
        if (Input.anyKeyDown && !hasPaused && CurrentState == GameState.InGame)
        {
            List<Notes> correct = CheckNotes(offset);
            List<Notes> early = CheckNotes(offset + 0.1f);
            List<Notes> late = CheckNotes(offset - 0.1f);

            InputManager.Instance.PressKey(Direction.Left);
            InputManager.Instance.PressKey(Direction.Right);
            InputManager.Instance.PressKey(Direction.Up);
            InputManager.Instance.PressKey(Direction.Down);
            
            if (correct.Count > 0)
            { 
                for(int i = 0; i < correct.Count; i++)
                {
                    if (Input.GetKeyDown(InputManager.Instance.GetKey(correct[i].direction)))
                    {
                        float noteSpeed = correct[i].size / 2 / correct[i].duration;
                        float pitch = (correct[i].size / 2 - offset) / noteSpeed;
                        Debug.Log(string.Format("{0}, {1}, {2}", pitch, Time.time, correct[i].summonedTime));
                        Debug.Log(pitch - (Time.time - correct[i].summonedTime));
                        DOTween.Kill(correct[i].transform);
                        if (correct[i].GetComponent<Notes>().isLast)
                        {
                            UIManager.Instance.ShowResult("Success");
                        }
                        PoolManager.Instance.DeSpawn(correct[i].gameObject);
                    }
                }
            }
        }
    }

    public void Fail()
    {
        
        UIManager.Instance.SetLifeText(--Life);
        if (Life <= 0)
        {
            if (hasPaused) return;
            hasPaused = true;
            UIManager.Instance.ShowResult("Failed");
            return;
        }

    }

    public void PauseGame(bool pause)
    {
        hasPaused = pause;
    }

    private List<Notes> CheckNotes(float size)
    {
        List<Notes> result = new List<Notes>();
        foreach (var notes in Physics2D.OverlapCircleAll(Vector2.zero, size, LayerMask.GetMask("Notes")))
        {
            result.Add(notes.GetComponent<Notes>());
        }
        return result;
        
    }

    private IEnumerator Ready()
    {
        correctNotes = 0;
        hasPaused = false;
        StartCoroutine(UIManager.Instance.ShowToolTips());
        yield return new WaitForSeconds(5);
        StartCoroutine(Play(FileManager.Instance.pattern));
    }


    private IEnumerator Play(Patterns patterns)
    {
        for(int i =  0; i < patterns.noteCnt; i++)
        {
            if (hasPaused) yield break;
            Notes note = PoolManager.Instance.Pool(originalNote).GetComponent<Notes>();
            NoteController.Bigger(note, patterns.size[i], patterns.duration[i], patterns.direction[i], patterns.isLast[i]);
            yield return new WaitForSeconds(patterns.second[i]);
        }
    }

    public static void ChangeState(GameState state)
    {
        CurrentState = state;
    }
}
