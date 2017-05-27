using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEngine
{
    public class ForeGroundPainter : MonoBehaviour
    {
        private MeshRenderer _meshRenderer;
        private Texture2D _mask;

        private const int _maskSize = 512;

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _mask = new Texture2D(_maskSize, _maskSize);
            _meshRenderer.material.SetTexture("_Mask", _mask);
        }

        public void PaintSquare(Vector2 topRight, Vector2 bottomLeft)
        {
            var root = GameManager.Instance.RootCell;

            topRight = new Vector2((topRight.x - root.bottomLeft.x) / root.Extend.x, (topRight.y - root.bottomLeft.y) / root.Extend.y);
            bottomLeft = new Vector2((bottomLeft.x - root.bottomLeft.x) / root.Extend.x, (bottomLeft.y - root.bottomLeft.y) / root.Extend.y);

            for (int i = Mathf.FloorToInt(bottomLeft.x * _maskSize); i < Mathf.CeilToInt(topRight.x * _maskSize); i++)
            {
                for (int j = Mathf.FloorToInt(bottomLeft.y * _maskSize); j < Mathf.CeilToInt(topRight.y * _maskSize); j++)
                {
                    _mask.SetPixel(i, j, Color.white);
                }
            }
            _mask.Apply();
        }
    }
}