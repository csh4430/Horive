using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoSingleton<SoundManager>
{
    public AudioMixer MasterMixer { get; private set; }
    public AudioSource MusicSource { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        MusicSource = GameObject.Find("Music Source").GetComponent<AudioSource>();
        DontDestroyOnLoad(MusicSource.transform.parent.gameObject);
    }

    public override void Initialize()
    {
        base.Initialize();

        MasterMixer = Resources.Load("Audio/MainAudio") as AudioMixer;
    }

    public void AudioControl(string key,float value)
    {
        switch (key)
        {
            case "Master":
                FileManager.Instance.setting.audioSetting.master = value;
                break;
            case "Music":
                FileManager.Instance.setting.audioSetting.music = value;
                break;
            case "HitSound":
                FileManager.Instance.setting.audioSetting.hitSound = value;
                break;
            case "Interaction":
                FileManager.Instance.setting.audioSetting.interaction = value;
                break;
        }
        MasterMixer.SetFloat(key, value * (2f / 5f) - 40f);
        Setting load = FileManager.Instance.LoadJsonFile<Setting>(Application.streamingAssetsPath + "/Save", "Setting");
        FileManager.Instance.SaveJson(Application.streamingAssetsPath + "/Save", "Setting", new Setting(load.keySetting, FileManager.Instance.setting.audioSetting));
    }

    public void PlayMusic(string name)
    {
        MusicSource.clip = (AudioClip)Resources.Load("Music/" + name);
        MusicSource.Play();
    }

    public void StopMusic()
    {
        MusicSource.clip = null;
        MusicSource.Stop();
    }
}
