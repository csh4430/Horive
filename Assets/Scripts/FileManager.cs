using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class FileManager : MonoSingleton<FileManager>
{
    public Setting setting = new Setting();
    public Patterns pattern = new Patterns();
    public List<Patterns> patterns = new List<Patterns>();

    protected override void Awake()
    {
        base.Awake();
        
    }

    public override void Initialize()
    {
        base.Initialize();

        setting = LoadJsonFile<Setting>(Application.streamingAssetsPath + "/Save", "Setting");

        patterns.Clear();
        for (int i = 0; i < 4; i++)
        {
            patterns.Add(LoadJsonFile<Patterns>(Application.streamingAssetsPath + "/Save", "Pattern_" + i.ToString()));
        }
        SoundManager.Instance.audioClips.Clear();
        foreach (var a in patterns)
        {
            if (a.title != "Tutorial")
            SoundManager.Instance.audioClips.Add(a.title, (AudioClip)Resources.Load("Music/" + a.title));
        }
        foreach (KeySetting keySet in setting.keySetting)
        {
            if (GameManager.CurrentState != GameManager.GameState.Init)
                UIManager.Instance.SetKey(keySet.direction, keySet.keyCode);
            InputManager.Instance.SetKey(keySet.direction, keySet.keyCode);
        }
        if (GameManager.CurrentState != GameManager.GameState.Init)
        {
            UIManager.Instance.SetOffsetUI(setting.offset);
            UIManager.Instance.SetSlider("Master", setting.audioSetting.master);
            UIManager.Instance.SetSlider("Music", setting.audioSetting.music);
            UIManager.Instance.SetSlider("Interaction", setting.audioSetting.interaction);
        }
        if (GameManager.CurrentState == GameManager.GameState.InGame)
        {
            pattern = LoadJsonFile<Patterns>(Application.streamingAssetsPath + "/Save", "Pattern_" + GameManager.Instance.Tag.ToString());
        }
    }

    private void Start()
    {
        SoundManager.Instance.AudioControl("Master", setting.audioSetting.master);
        SoundManager.Instance.AudioControl("Music", setting.audioSetting.music);
        SoundManager.Instance.AudioControl("Interaction", setting.audioSetting.interaction);
        
    }

    public void SaveJson<T>(string createPath, string fileName, T value)
    {
        FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", createPath, fileName), FileMode.Create);
        string json = JsonUtility.ToJson(value, true);
        byte[] data = Encoding.UTF8.GetBytes(json);
        fileStream.Write(data, 0, data.Length);
        fileStream.Close();
    }

    public T LoadJsonFile<T>(string loadPath, string fileName) where T : new()
    {
        if (File.Exists(string.Format("{0}/{1}.json", loadPath, fileName)))
        {
            FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", loadPath, fileName), FileMode.Open);
            byte[] data = new byte[fileStream.Length];
            fileStream.Read(data, 0, data.Length);
            fileStream.Close();
            string jsonData = Encoding.UTF8.GetString(data);
            return JsonUtility.FromJson<T>(jsonData);
        }
        SaveJson(loadPath, fileName, new T());
        return LoadJsonFile<T>(loadPath, fileName);
    }
}
