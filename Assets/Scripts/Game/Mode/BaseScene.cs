using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;

public abstract class BaseScene : GameCore
{
    protected abstract void OnScenePreload();

    private void Awake()
    {
        OnScenePreload();
    }

    protected override void OnModeStart()
    {
    }

    protected override void OnModeUpdate()
    {

    }

    protected override void OnModeFixedUpdate()
    {

    }

    protected override void OnModeDestroy()
    {

    }
}