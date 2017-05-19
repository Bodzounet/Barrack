using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace GameEngine
{
    public class Cell : Square
    {
        private List<Ball> _balls = new List<Ball>();
        public ReadOnlyCollection<Ball> Balls
        {
            get { return _balls.AsReadOnly(); }
        }

        public void DivideCell(Vector2 spliterPosition, RayLauncher.RayDirection direction)
        {
            if (childrens != null)
                throw new InvalidOperationException("Cannot divide a square already divided");

            if (Filled)
                throw new InvalidOperationException("Cannot divide a filled square");

            Cell s1, s2;

            if (direction == RayLauncher.RayDirection.Horizontal)
            {
                s1 = _CreateChildCell(new Vector2(bottomLeft.x, spliterPosition.y + Wall.WallWidth / 2), topRight);
                s2 = _CreateChildCell(bottomLeft, new Vector2(topRight.x, spliterPosition.y - Wall.WallWidth / 2));
                wall = _CreateChildWall(new Vector2(bottomLeft.x, spliterPosition.y - Wall.WallWidth / 2), new Vector2(topRight.x, spliterPosition.y + Wall.WallWidth / 2));
            }
            else
            {
                s1 = _CreateChildCell(bottomLeft, new Vector2(spliterPosition.x - Wall.WallWidth / 2, topRight.y));
                s2 = _CreateChildCell(new Vector2(spliterPosition.x + Wall.WallWidth / 2, bottomLeft.y), topRight);
                wall = _CreateChildWall(new Vector2(spliterPosition.x - Wall.WallWidth / 2, bottomLeft.y), new Vector2(spliterPosition.x + Wall.WallWidth / 2, topRight.y));
            }

            childrens = new Tuple<Cell>(s1, s2);

            foreach (var v in _balls)
            {
                if (s1.Contains(v.transform.position))
                {
                    s1._balls.Add(v);
                }
                else
                {
                    s2._balls.Add(v);
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

        private Cell _CreateChildCell(Vector2 bottomLeft, Vector2 topRight)
        {
            GameObject newCell = new GameObject();
            Cell cell = newCell.AddComponent<Cell>();
            cell.Ctor(bottomLeft, topRight);
            return cell;
        }

        private Wall _CreateChildWall(Vector2 bottomLeft, Vector2 topRight)
        {
            GameObject newWall = new GameObject();
            Wall wall = newWall.AddComponent<Wall>();
            wall.Ctor(bottomLeft, topRight);
            return wall;
        }
    }
}