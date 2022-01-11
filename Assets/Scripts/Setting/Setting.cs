using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Setting
{
    public List<KeySetting> keySetting = new List<KeySetting>();
    public AudioSetting audioSetting;

    public Setting()
    {
        keySetting.Clear();
        keySetting.Add(new KeySetting(Direction.Left));
        keySetting.Add(new KeySetting(Direction.Right));
        keySetting.Add(new KeySetting(Direction.Up));
        keySetting.Add(new KeySetting(Direction.Down));
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
}
