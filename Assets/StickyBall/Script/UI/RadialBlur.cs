using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class RadialBlur : MonoBehaviour
{
    [SerializeField]
    private Shader _shader;
    [SerializeField, Range(4, 16)]
    private int _sampleCount = 8;
    public int sampleCount
    {
        get => _sampleCount;
        set
        {
            if(value >= 4 && value <= 16) { _sampleCount = value; }
        }
    }
    [SerializeField, Range(0.0f, 1.0f)]
    private float _strength = 0.5f;
    public float strength
    {
        get => _strength;
        set
        {
            if (value >= 0.0f && value <= 1.0f) { _strength = value; }
        }
    }
    private Material _material;

    private void OnRenderImage(RenderTexture source, RenderTexture dest)
    {
        if (_material == null)
        {
            if (_shader == null)
            {
                Graphics.Blit(source, dest);
                return;
            }
            else
            {
                _material = new Material(_shader);
            }
        }
        _material.SetInt("_SampleCount", _sampleCount);
        _material.SetFloat("_Strength", _strength);
        Graphics.Blit(source, dest, _material);
    }
}
