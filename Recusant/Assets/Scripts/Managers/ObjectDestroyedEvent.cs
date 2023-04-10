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
        Debug.Log("REEEEE!!!");
        OnDestroyed.Invoke();
    }

    private void OnDestroy()
    {
        Debug.Log("Ded");
        //OnDestroyed.Invoke();
    }
}
