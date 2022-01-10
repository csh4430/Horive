using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoSingleton<InputManager>
{
    public Dictionary<Direction, KeyCode> keySetDict = new Dictionary<Direction, KeyCode>();
    public Dictionary<KeyCode, Direction> dirSetDict = new Dictionary<KeyCode, Direction>();

    public void SetKey(Direction direction, KeyCode key)
    {
        keySetDict[direction] = key;
        dirSetDict[key] = direction;
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
}
