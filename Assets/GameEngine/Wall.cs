using UnityEngine;

namespace GameEngine
{
    public class Wall : Square
    {
        public const float WallWidth = 0.1f;

        public override void Ctor(Vector2 bottomLeft, Vector2 topRight)
        {
            base.Ctor(bottomLeft, topRight);
            transform.position = (topRight + bottomLeft) / 2;
            BoxCollider2D col = gameObject.AddComponent<BoxCollider2D>();
            col.size = new Vector2((topRight.x - bottomLeft.x), (topRight.y - bottomLeft.y));
        }
    }
}
