using Core;
using Lean.Common;
using Lean.Touch;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] LeanManualTranslate m_Translate;
    [SerializeField] LeanMultiUpdate m_Update;
    [SerializeField] LeanPinchCamera m_Pinch;

    [SerializeField] float defaultZoom = 9.6f;
    [SerializeField] float unAnchorZoom = 20f;

    [SerializeField] Vector3 unAnchorPos = Vector3.zero;
    public void Init()
    {
        GameCore.Event.Subscribe<AnchorEventArgs>(OnAnchorEventHandler);
        GameCore.Event.Subscribe<UnAnchorEventArgs>(OnUnAnchorEventHandler);
    }

    private void OnDestroy()
    {
        GameCore.Event.Unsubscribe<AnchorEventArgs>(OnAnchorEventHandler);
        GameCore.Event.Unsubscribe<UnAnchorEventArgs>(OnUnAnchorEventHandler);
    }

    private void OnUnAnchorEventHandler(object sender, IEventArgs e)
    {
        this.transform.localPosition = unAnchorPos;
        m_Pinch.Zoom = unAnchorZoom;
        m_Translate.enabled = false;
        m_Update.enabled = false;
    }

    private void OnAnchorEventHandler(object sender, IEventArgs e)
    {
        m_Pinch.Zoom = defaultZoom;
        m_Translate.enabled = true;
        m_Update.enabled = true;
    }
}
