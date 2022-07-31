using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Setting
{
    public List<KeySetting> keySetting = new List<KeySetting>();
    public float offset;
    public AudioSetting audioSetting;


    public Setting()
    {
        keySetting.Clear();
        keySetting.Add(new KeySetting(Direction.Left));
        keySetting.Add(new KeySetting(Direction.Right));
        keySetting.Add(new KeySetting(Direction.Up));
        keySetting.Add(new KeySetting(Direction.Down));
        offset = 0f;
        audioSetting = new AudioSetting();
    }

    public Setting(Dictionary<Direction, KeyCode> keySetDict)
    {
        keySetting.Clear();

        foreach (var a in keySetDict)
        {
            keySetting.Add(new KeySetting(a.Key, a.Value));
        }
    }

    public Setting(AudioSetting audioSetting)
    {
        this.audioSetting = audioSetting;
    }


    public Setting(Dictionary<Direction, KeyCode> keySetDict, float offset, AudioSetting audioSetting)
    {
        keySetting.Clear();

        foreach (var a in keySetDict)
        {
            keySetting.Add(new KeySetting(a.Key, a.Value));
        }
        this.offset = offset;
        this.audioSetting = audioSetting;

    }

    public Setting(List<KeySetting> keySetting, float offset, AudioSetting audioSetting)
    {
        this.keySetting =   keySetting;
        this.offset = offset;
        this.audioSetting = audioSetting;
    }
}
