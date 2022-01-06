using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notes : MonoBehaviour
{
    public float summonedTime = 0f;
    public float duration = 0f;

    private void OnEnable()
    {
        summonedTime = Time.time;
    }

    private void OnDisable()
    {
        summonedTime = 0f;
        duration = 0f;
    }
}
