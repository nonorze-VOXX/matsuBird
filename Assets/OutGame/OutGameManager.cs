using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutGameManager : Singleton<OutGameManager>
{
    protected override void _Awake()
    {
        base._Awake();
    }

    public void StartGame()
    {
        Debug.Log("press space");
    }
}
