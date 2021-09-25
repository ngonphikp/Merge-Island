using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Joystick joystick;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        rb.isKinematic = true;
        rb.detectCollisions = false;
    }

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
        rb.isKinematic = false;
        rb.detectCollisions = true;
    }

    private void OnAnchorEventHandler(object sender, IEventArgs e)
    {
        rb.isKinematic = true;
        rb.detectCollisions = false;
    }

    public void FixedUpdate()
    {
        if (!rb.isKinematic)
        {
            Vector2 direction = Vector2.up * joystick.Vertical + Vector2.right * joystick.Horizontal;
            rb.AddForce(direction * speed * Time.fixedDeltaTime, ForceMode.Force);
        }
    }
}