using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : MonoSingleton<InputManager>
{
    private KeyCode settingKey = KeyCode.Escape;
    public Dictionary<Direction, KeyCode> keySetDict = new Dictionary<Direction, KeyCode>();
    public Dictionary<KeyCode, Direction> dirSetDict = new Dictionary<KeyCode, Direction>();
    private bool isHoldingKey = false;

    private Direction dir = Direction.None;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && GameManager.CurrentState == GameManager.GameState.Init)
        {
            Debug.Log(1);
            NoteController.Bigger(PoolManager.Instance.Pool(GameManager.Instance.originalNote).GetComponent<Notes>(), 5, 1f, Camera.main.ScreenToWorldPoint(Input.mousePosition));
            UIManager.Instance.Fade(true, () =>
            {
                GameManager.ChangeState(GameManager.GameState.Select);
                SceneManager.LoadScene("Select");
                UIManager.Instance.Fade(false, null);
            }); 
        }
        if (Input.GetKeyDown(settingKey) && GameManager.CurrentState != GameManager.GameState.Init)
        {
            UIManager.Instance.SetPanel("Setting");
        }
    }
    public void PressKey(Direction dir)
    {
        if (Input.GetKeyDown(keySetDict[dir]))
        {
            UIManager.Instance.ShowKeyInput(dir);
        }
    }

    public void SetKey(Direction direction, KeyCode key)
    {
        keySetDict[direction] = key;
        dirSetDict[key] = direction;
        FileManager.Instance.SaveJson(Application.dataPath + "/Save", "Setting", new Setting(keySetDict, FileManager.Instance.setting.audioSetting));
    }

    public KeyCode GetKey(Direction direction)
    {
        return keySetDict[direction];
    }

    public Direction GetDirection(KeyCode key)
    {
        foreach(var a in dirSetDict.Keys)
        {
            if(a == key)
            {
                return dirSetDict[key];
            }
        }

        return Direction.None;
    }

    public void ChangeKey(Direction direction)
    {
        dir = direction;
    }

    private void OnGUI()
    {
        Event e = Event.current;
        if (e.type == EventType.KeyDown && dir != Direction.None && !isHoldingKey)
        {
            isHoldingKey = true;
            Debug.Log(e.keyCode.ToString());
            SetKey(dir, e.keyCode);
            UIManager.Instance.SetKey(dir, e.keyCode);
            dir = Direction.None;
        }
        if (e.type == EventType.KeyUp && isHoldingKey)
        {
            isHoldingKey = false;
        }
    }
}
