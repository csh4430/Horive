using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NoteController
{ 
    public static void Bigger(Notes note, float size, float duration, Direction direction, bool isLast)
    {
        if (GameManager.CurrentState != GameManager.GameState.InGame) return;
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
            if (note.isLast && GameManager.Instance.Life > 0)
            {
                GameManager.Instance.PauseGame(false);

                FileManager.Instance.pattern.HasCleared = true;
                FileManager.Instance.SaveJson(Application.streamingAssetsPath + "/Save", "Pattern_" + FileManager.Instance.pattern.Tag.ToString(), FileManager.Instance.pattern);
                UIManager.Instance.ShowResult("Success");
            }
        }
        );
    }

    public static void Bigger(Notes note, float size, float duration, Vector2 position)
    {
        note.transform.position = position;

        note.transform.DOScale(Vector2.one * size, duration);
    }
}
