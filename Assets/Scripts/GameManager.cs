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
        Notes a = Instantiate(originalNote).GetComponent<Notes>();
        NoteController.Bigger(a, 10, 1, "Up");
        a.duration = 1;
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            if(CheckNote(offset) != null)
            {
                float duration = CheckNote(offset).duration;
                float summonedTime = CheckNote(offset).summonedTime;
                Debug.Log(duration - Time.time - summonedTime);
            }
        }
    }




    private Notes CheckNote(float size)
    {
        return Physics2D.OverlapCircle(centerObject.transform.position, size, LayerMask.GetMask("Notes"))?.GetComponent<Notes>();
    }
}
