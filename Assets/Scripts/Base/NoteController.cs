using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NoteController
{
    public static void Bigger(Notes note, float size, float duration, string direction)
    {
        switch (direction)
        {
            case "Right":
                note.transform.position = new Vector2(size / 2, 0);
                break;
            case "Left":
                note.transform.position = new Vector2(-size / 2, 0);
                break;
            case "Up":
                note.transform.position = new Vector2(0, size / 2);
                break;
            case "Down":
                note.transform.position = new Vector2(0, -size / 2);
                break;
        }
        note.transform.DOScale(Vector2.one * size, duration).SetEase(Ease.Linear).onComplete += () =>
        {
            note.gameObject.SetActive(false);
        };
    }
}
