using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForeGroundPainter : MonoBehaviour
{
    private MeshRenderer _meshRenderer;
    private Texture2D _mask;

    private const int _maskSize = 256;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _mask = new Texture2D(_maskSize, _maskSize);
        _meshRenderer.material.SetTexture("_Mask", _mask);
    }

    /// <summary>
    /// TopRight and BottomLeft have to be clamped between [0..1]
    /// </summary>
    /// <param name="topRight"></param>
    /// <param name="bottomLeft"></param>
    public void PaintSquare(Vector2 topRight, Vector2 bottomLeft)
    {
        for (int i = Mathf.FloorToInt(bottomLeft.x * _maskSize); i < Mathf.FloorToInt(topRight.x * _maskSize); i++)
        {
            for (int j = Mathf.FloorToInt(bottomLeft.y * _maskSize); j < Mathf.FloorToInt(topRight.y * _maskSize); j++)
            {
                _mask.SetPixel(i, j, Color.white);
            }
        }
        _mask.Apply();
    }
}
