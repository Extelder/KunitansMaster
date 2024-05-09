using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InvokeOnEnable : MonoBehaviour
{
    public UnityEvent Event;

    private void OnEnable()
    {
        Event?.Invoke();
    }
}