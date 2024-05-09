using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public bool TimeIsStopped;

    public static TimeManager Instance { get; private set; }

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            return;
        }

        Destroy(this);
    }

    public void ContinueTime()
    {
        TimeIsStopped = false;
        Time.timeScale = 1f;
    }

    public void StopTime()
    {
        TimeIsStopped = true;
        Time.timeScale = 0.3f;
    }
}