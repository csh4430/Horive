
[System.Serializable]
public class AudioSetting
{
    public float master;
    public float music;
    public float interaction;

    public AudioSetting()
    {
        master = 50;
        music = 50;
        interaction = 50;
    }

    public AudioSetting(float master, float music, float interaction)
    {
        this.master = master;
        this.music = music;
        this.interaction = interaction;
    }
}
