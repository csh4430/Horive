using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoSingleton<InputManager>
{
    public Dictionary<Direction, KeyCode> keySetDict = new Dictionary<Direction, KeyCode>();

    public void SetKey(Direction direction, KeyCode key)
    {
        keySetDict[direction] = key;
    }

    public KeyCode GetKey(Direction direction)
    {
        return keySetDict[direction];
    }
}
