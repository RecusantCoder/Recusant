using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectDestroyedEvent : MonoBehaviour
{
    public UnityEvent OnDestroyed;

    private void Awake()
    {
        OnDestroyed.Invoke();
    }

    private void OnDestroy()
    {
        Debug.Log("ObjectDestroyedEvent says " + gameObject.name + " was destroyed.");
        //OnDestroyed.Invoke();
    }
}
