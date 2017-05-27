using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameEngine
{
    public class Cell
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

                GameManager gm = GameManager.Instance;

                if (gm.Shark != null && Contains(gm.Shark.transform.position))
                    gm.Shark.Die();

                gm.ForegroundPainter.PaintSquare(topRight, bottomLeft);
                gm.Progression.RegisterNewFilledCell(this);
            }
        }

        public Tuple<Cell> Childrens
        {
            get;
            private set;
        }

        public Cell Wall
        {
            get;
            private set;
        }


        private List<Ball> _balls = new List<Ball>();
        public List<Ball> Balls
        {
            get { return new List<Ball>(_balls); }
        }

        public Cell(Vector2 bottomLeft, Vector2 topRight)
        {
            this.topRight = topRight;
            this.bottomLeft = bottomLeft;
        }

        public void DivideCell(Vector2 spliterPosition, RayLauncher.RayDirection direction)
        {
            if (Childrens != null)
                throw new InvalidOperationException("Cannot divide a square already divided");

            if (Filled)
                throw new InvalidOperationException("Cannot divide a filled square");

            Cell s1, s2;

            if (direction == RayLauncher.RayDirection.Horizontal)
            {
                s1 = new Cell(new Vector2(bottomLeft.x, spliterPosition.y + WallBuilder.WallWidth / 2), topRight);
                s2 = new Cell(bottomLeft, new Vector2(topRight.x, spliterPosition.y - WallBuilder.WallWidth / 2));

                Wall = new Cell(new Vector2(bottomLeft.x, spliterPosition.y - WallBuilder.WallWidth / 2), new Vector2(topRight.x, spliterPosition.y + WallBuilder.WallWidth / 2));
            }
            else
            {
                s1 = new Cell(bottomLeft, new Vector2(spliterPosition.x - WallBuilder.WallWidth / 2, topRight.y));
                s2 = new Cell(new Vector2(spliterPosition.x + WallBuilder.WallWidth / 2, bottomLeft.y), topRight);

                Wall = new Cell(new Vector2(spliterPosition.x - WallBuilder.WallWidth / 2, bottomLeft.y), new Vector2(spliterPosition.x + WallBuilder.WallWidth / 2, topRight.y));
            }

            Childrens = new Tuple<Cell>(s1, s2);
            Wall.Filled = true;

            foreach (var v in _balls)
            {
                if (s1.Contains(v.transform.position))
                {
                    s1.AddBall(v);
                }
                else
                {
                    s2.AddBall(v);
                }
            }

            if (s1.Balls.Count == 0)
                s1.Filled = true;

            if (s2.Balls.Count == 0)
                s2.Filled = true;

            _balls = null;
        }

        public void AddBall(Ball b)
        {
            _balls.Add(b);
            b.ParentCell = this;
        }

        public void RemoveBall(Ball b)
        {
            if (!_balls.Contains(b))
                throw new InvalidOperationException("The ball you try to remove is not contained by this square");

            _balls.Remove(b);
            if (_balls.Count == 0)
            {
                Filled = true;
            }
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

        public Vector2 Extend
        {
            get { return topRight - bottomLeft; }
        }
    }
}