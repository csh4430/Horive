using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject centerObject = null;
    [SerializeField] GameObject originalNote = null;
    [SerializeField] float offset = 0;

    void Start()
    {
        StartCoroutine(Ready());
        InputManager.Instance.SetKey(Direction.Left, KeyCode.D);
        InputManager.Instance.SetKey(Direction.Right, KeyCode.K);
        InputManager.Instance.SetKey(Direction.Up, KeyCode.F);
        InputManager.Instance.SetKey(Direction.Down, KeyCode.J);
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            Notes a = CheckNote(offset);
            if (a != null)
            {
                if (Input.GetKeyDown(InputManager.Instance.GetKey(a.direction)))
                {
                
                    float noteSpeed = a.size / 2 / a.duration;
                    float pitch = (a.size / 2 - offset) / noteSpeed;
                    Debug.Log(string.Format("{0}, {1}, {2}", pitch, Time.time, a.summonedTime));
                    Debug.Log(pitch - (Time.time - a.summonedTime));
                    PoolManager.Instance.DeSpawn(a.gameObject);
                }
            }
        }
    }

    private Notes CheckNote(float size)
    {
        return Physics2D.OverlapCircle(centerObject.transform.position, size, LayerMask.GetMask("Notes"))?.GetComponent<Notes>();
    }

    private IEnumerator Ready()
    {
        StartCoroutine(UIManager.Instance.ShowToolTips());
        yield return new WaitForSeconds(4);
        StartCoroutine(Play());
    }


    private IEnumerator Play()
    {
        yield return new WaitForSeconds(0.5f);
        Notes a = PoolManager.Instance.Pool(originalNote).GetComponent<Notes>();
        NoteController.Bigger(a, 5, 0.5f, Direction.Left);
        yield return new WaitForSeconds(0.5f);
        Notes c = PoolManager.Instance.Pool(originalNote).GetComponent<Notes>();
        NoteController.Bigger(c, 5, 0.5f, Direction.Up);
        yield return new WaitForSeconds(0.5f);
        Notes d = PoolManager.Instance.Pool(originalNote).GetComponent<Notes>();
        NoteController.Bigger(d, 5, 0.5f, Direction.Down);
        yield return new WaitForSeconds(0.5f);
        Notes b = PoolManager.Instance.Pool(originalNote).GetComponent<Notes>();
        NoteController.Bigger(b, 5, 0.5f, Direction.Right);
    }
}
