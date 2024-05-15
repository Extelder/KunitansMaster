using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InvokeOnDisable : MonoBehaviour
{
    public UnityEvent DisableEvent;

    private void OnDisable()
    {
       DisableEvent?.Invoke();
    }
}
