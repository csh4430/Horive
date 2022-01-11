using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class FileManager : MonoSingleton<FileManager>
{
    [SerializeField] private Setting setting = new Setting();

    private void Start()
    {
        setting = LoadJsonFile<Setting>(Application.dataPath + "/Save", "Setting");
        foreach(KeySetting keySet in setting.keySetting)
        {
            UIManager.Instance.SetKey(keySet.direction, keySet.keyCode);
            InputManager.Instance.SetKey(keySet.direction, keySet.keyCode);
        }
    }

    public void SaveJson<T>(string createPath, string fileName, T value)
    {
        FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", createPath, fileName), FileMode.Create);
        string json = JsonUtility.ToJson(value, true);
        byte[] data = Encoding.UTF8.GetBytes(json);
        fileStream.Write(data, 0, data.Length);
        fileStream.Close();
    }

    T LoadJsonFile<T>(string loadPath, string fileName) where T : new()
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
