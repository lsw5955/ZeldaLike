﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SignalListener : MonoBehaviour
{
    public Signaler signal;
    public UnityEvent signalEvent;

    public void OnSignalRaised()
    {
        signalEvent.Invoke();
    }

    private void OnEnable()
    {
        Debug.Log("我开始注册到Signal");
        signal.RegisterListner(this);
    }

    private void OnDisable()
    {
        signal.DeRegisterListner(this);
    }
}
