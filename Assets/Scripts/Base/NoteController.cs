using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NoteController
{
    public static void Bigger(Notes note, float size, float duration, Direction direction)
    {
        note.SetPitch(size, duration, direction);
        switch (direction)
        {
            case Direction.Right:
                note.transform.position = new Vector2(size / 2, 0);
                break;
            case Direction.Left:
                note.transform.position = new Vector2(-size / 2, 0);
                break;
            case Direction.Up:
                note.transform.position = new Vector2(0, size / 2);
                break;
            case Direction.Down:
                note.transform.position = new Vector2(0, -size / 2);
                break;
        }
        note.transform.DOScale(Vector2.one * size, duration).onComplete += () =>
        {
            note.gameObject.transform.localScale = Vector3.zero;
            if (!note.gameObject.activeInHierarchy) return;
            Debug.Log(Time.time - note.summonedTime);
            PoolManager.Instance.DeSpawn(note.gameObject);
        };
    }
}
