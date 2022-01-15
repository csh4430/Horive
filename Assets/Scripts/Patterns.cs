[System.Serializable]
public class Patterns
{
    public int Tag;
    public string title;
    public string composer;
    public int noteCnt;
    public float[] second;
    public float[] size;
    public float[] duration;
    public Direction[] direction;
    public bool[] isLast;

    public Patterns()
    {
        Tag = 0;
        title = "Tutorial";
        composer = "Developer";
        noteCnt = 4;
        second = new float[4];
        size = new float[4];
        duration = new float[4];
        direction = new Direction[4];
        isLast = new bool[4];
    }
}
