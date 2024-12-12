using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyHelper : MonoBehaviour
{
    public float gravity = 0.1f;
    public float offset = 0.2f;
    public Terrain terrain;
    private float _lastY = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if (Math.Abs(_lastY - transform.position.y) < 0.001f)
        {
            transform.position = new Vector3(transform.position.x, Math.Max(transform.position.y - gravity * Time.deltaTime, offset + terrain.SampleHeight(new Vector3(transform.position.x, 0f, transform.position.z))), transform.position.z);
        }
        _lastY = transform.position.y;
    }
}
