using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class FlyHelper : MonoBehaviour
{
    public InputActionReference freezeHeightAction;
    public DynamicMoveProvider dynamicMoveProvider;
    public float gravity = 0.1f;
    public float offset = 0.2f;
    public Terrain terrain;
    private float _lastY = 0f;

    float currGravity;
    // Start is called before the first frame update
    void Start()
    {
        freezeHeightAction.action.started += ctx => ToggleFreezeHeight(true);
        freezeHeightAction.action.canceled += ctx => ToggleFreezeHeight(false);
        currGravity = gravity;
    }

    void ToggleFreezeHeight(bool isFrozen)
    {
        dynamicMoveProvider.enableFly = !isFrozen;
        dynamicMoveProvider.useGravity = !isFrozen;
        currGravity = isFrozen ? 0f : gravity;
    }

    private void FixedUpdate()
    {
        if (Math.Abs(_lastY - transform.position.y) < 0.001f)
        {
            transform.position = new Vector3(transform.position.x, Math.Max(transform.position.y - currGravity * Time.deltaTime, offset + terrain.SampleHeight(new Vector3(transform.position.x, 0f, transform.position.z))), transform.position.z);
        }
        _lastY = transform.position.y;
    }
}
