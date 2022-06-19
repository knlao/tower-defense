using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserLineController : MonoBehaviour
{
    public Texture[] textures;
    public float fps;

    private LineRenderer _lineRenderer;
    private int _animationStep;
    private float _fpsCounter;

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        _fpsCounter += Time.deltaTime;
        if (_fpsCounter >= 1 / fps)
        {
            _animationStep++;
            if (_animationStep >= textures.Length)
                _animationStep = 0;
            _lineRenderer.material.SetTexture("_MainTex", textures[_animationStep]);
            _fpsCounter = 0;
        }
        //image.tile.x = line.magnitude;
    }
}
