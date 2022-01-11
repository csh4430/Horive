using System;
using UnityEngine;

[Serializable]
public class KeySetting
{
    public Direction direction;
    public KeyCode keyCode;

    public KeySetting(Direction dir)
    {
        direction = dir;
        switch (dir)
        {
            case Direction.Left:
                keyCode = KeyCode.D;
                break;
            case Direction.Right:
                keyCode = KeyCode.K;
                break;
            case Direction.Up:
                keyCode = KeyCode.F;
                break;
            case Direction.Down:
                keyCode = KeyCode.L;
                break;
        }
    }

    public KeySetting(Direction dir, KeyCode key)
    {
        direction = dir;
        keyCode = key;
    }
}
