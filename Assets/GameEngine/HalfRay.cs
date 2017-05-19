using System;
using System.Collections;
using UnityEngine;

namespace GameEngine
{
    public class HalfRay : MonoBehaviour
    {
        public enum RayDirection
        {
            Up = 0,
            Down = 2,
            Left = 1,
            Right = 3
        }

        public event Action OnReachAWall;
        public event Action OnHit;

        private LineRenderer _ray;
        private BoxCollider2D _collider;

        public float speed = 1f;

        public bool isOver { get; private set; }

        private void Awake()
        {
            _ray = GetComponent<LineRenderer>();
            _collider = GetComponent<BoxCollider2D>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (OnHit != null)
                OnHit();
        }

        public void DrawRay(Vector3 startingPos, RayDirection direction)
        {
            _ray.SetPosition(0, startingPos);
            _ray.SetPosition(1, startingPos);
            StartCoroutine(Co_DrawRay(direction));
        }

        private IEnumerator Co_DrawRay(RayDirection direction)
        {
            Vector3 dir = Vector3.zero;
            switch (direction)
            {
                case RayDirection.Up: dir = Vector2.up; break;
                case RayDirection.Down: dir = Vector2.down; break;
                case RayDirection.Left: dir = Vector2.left; break;
                case RayDirection.Right: dir = Vector2.right; break;
            }

            modEven = (int)direction % 2;         // 0 for up and down, 1 otherwise
            modOdd = ((int)direction + 1) % 2;    // the opposite

            while (true)
            {
                float distance = speed * Time.deltaTime;
                var hit = Physics2D.Raycast(_ray.GetPosition(1), dir, distance, 1 << LayerMask.NameToLayer("Wall"));

                if (hit.collider == null)
                {
                    _ExtendRay(dir * distance);
                    yield return new WaitForEndOfFrame();
                }
                else
                {
                    _ExtendRay(dir * hit.distance);
                    isOver = true;
                    if (OnReachAWall != null)
                        OnReachAWall();

                    break;
                }
            }
        }

        private int modEven;
        private int modOdd;

        private void _ExtendRay(Vector3 extension)
        {
            _ray.SetPosition(1, _ray.GetPosition(1) + extension);

            var diff = (_ray.GetPosition(1) - _ray.GetPosition(0));
            _collider.offset = new Vector2(diff.x / 2 * modEven, diff.y / 2 * modOdd);
            _collider.size = new Vector2(Wall.WallWidth * modOdd + Mathf.Abs(diff.x) * modEven, Wall.WallWidth * modEven + Mathf.Abs(diff.y) * modOdd);
        }
    }
}
