using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoSingleton<SoundManager>
{
    public AudioMixer MasterMixer { get; private set; }
    public AudioSource MusicSource { get; private set; }
    public AudioSource InteractionSource { get; private set; }

    public Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

    protected override void Awake()
    {
        base.Awake();
        MusicSource = GameObject.Find("Music Source").GetComponent<AudioSource>();
        DontDestroyOnLoad(MusicSource.transform.parent.gameObject);
        InteractionSource = GameObject.Find("InteractionSource").GetComponent<AudioSource>();
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
            case "Interaction":
                FileManager.Instance.setting.audioSetting.interaction = value;
                break;
        }
        MasterMixer.SetFloat(key, value * (2f / 5f) - 40f);
        Setting load = FileManager.Instance.LoadJsonFile<Setting>(Application.streamingAssetsPath + "/Save", "Setting");
        FileManager.Instance.SaveJson(Application.streamingAssetsPath + "/Save", "Setting", new Setting(load.keySetting, FileManager.Instance.setting.offset, FileManager.Instance.setting.audioSetting));
    }

    public void PlayMusic(string name)
    {
        MusicSource.clip = audioClips[name];
        MusicSource.Play();
    }

    public void StopMusic()
    {
        MusicSource.clip = null;
        MusicSource.Stop();
    }
}
