using UnityEngine;

public abstract class Square : MonoBehaviour
{
    public Vector2 topRight;
    public Vector2 bottomLeft;

    private bool _filled;
    public bool Filled
    {
        get { return _filled; }
        protected set
        {
            if (_filled)
                return;

            _filled = value;
            //TODO : onFilled
        }
    }

    [HideInInspector]
    public Tuple<Cell> childrens;
    [HideInInspector]
    public Wall wall;

    public virtual void Ctor(Vector2 bottomLeft, Vector2 topRight)
    {
        this.topRight = topRight;
        this.bottomLeft = bottomLeft;
    }

    public bool Contains(Vector2 pos)
    {
        return pos.x < topRight.x && pos.x > bottomLeft.x &&
            pos.y < topRight.y && pos.y > bottomLeft.y;
    }

    public float Area()
    {
        return (topRight.x - bottomLeft.x) * (topRight.y - bottomLeft.y);
    }
}
