using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Direction
{
    Left,
    Right,
    Up,
    Down,
    None
}

public class Notes : MonoBehaviour
{
    public float summonedTime { get; private set; }
    public float duration  { get; private set; }
    public float size { get; private set; }

    public Direction direction { get; private set; }
    

    public void SetPitch(float size, float duration, Direction direction)
    {
        this.size = size;
        this.duration = duration;
        this.direction = direction;
    }

    private void OnEnable()
    {
        summonedTime = Time.time;
    }

    private void OnDisable()
    {
        transform.localScale = Vector3.zero;
    }
}
