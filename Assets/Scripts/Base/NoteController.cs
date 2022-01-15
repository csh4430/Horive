using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NoteController
{
    private static Sequence sequence;
    public static void Bigger(Notes note, float size, float duration, Direction direction, bool isLast)
    {
        sequence = DOTween.Sequence().OnStart(() =>
        {
            note.transform.localScale = Vector3.zero;
            note.SetPitch(size, duration, direction, isLast);
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
           note.transform.DOScale(Vector2.one * size, duration).OnComplete(() =>
                {
                    note.transform.localScale = Vector3.zero;
                    
                    if (!note.gameObject.activeInHierarchy) return;
                    Debug.Log(Time.time - note.summonedTime);
                    PoolManager.Instance.DeSpawn(note.gameObject);
                    GameManager.Instance.Fail();
                }
           );
        });
    }

    public static void Bigger(Notes note, float size, float duration, Vector2 position)
    {
        note.transform.position = position;

        note.transform.DOScale(Vector2.one * size, duration);
    }

    public static void Stop()
    {
        sequence.Kill();
    }
}
