using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEngine
{
    public class RayLauncher : MonoBehaviour
    {
        public enum RayDirection
        {
            Horizontal = 0,
            Vertical = 1
        }
        private RayDirection _direction = RayDirection.Horizontal;
        public RayDirection Direction
        {
            get { return _direction; }
            private set
            {
                _direction = value;
            }
        }

        public bool IsShooting
        {
            get;
            private set;
        }

        public GameManager gameManager;

        public GameObject P_Ray;

        private Vector3 _previousMousePos;

        private void Start()
        {
            _previousMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Cursor.visible = false;
        }

        private void Update()
        {
            _UpdateRayLauncherPosition();
            _UpdateRayDirection();
            _Shoot();
        }

        private void _UpdateRayLauncherPosition()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 delta = Camera.main.ScreenToWorldPoint(Input.mousePosition) - _previousMousePos;
            Vector3 newPos = transform.position + delta;
            newPos = new Vector3(Mathf.Clamp(newPos.x, gameManager.RootCell.bottomLeft.x, gameManager.RootCell.topRight.x),
                Mathf.Clamp(newPos.y, gameManager.RootCell.bottomLeft.y, gameManager.RootCell.topRight.y));

            _previousMousePos = mousePos;
            transform.position = newPos;
        }

        private void _UpdateRayDirection()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Direction = (RayDirection)(((int)Direction + 1) % 2);
            }
        }

        private void _Shoot()
        {
            if (!IsShooting && Input.GetMouseButtonDown(0))
            {
                Cell emptyCell = null;
                if (_IsInAnEmptyCell(gameManager.RootCell, transform.position, out emptyCell))
                {
                    IsShooting = true;
                    GameObject go = GameObject.Instantiate(P_Ray, transform.position, Quaternion.identity);
                    Ray ray = go.GetComponent<Ray>();
                    ray.DrawRay(transform.position, Direction,
                    () =>
                    {
                        emptyCell.DivideCell(transform.position, Direction);
                        IsShooting = false;
                    },
                    () =>
                    {
                    // gameManager.Life--;
                    IsShooting = false;
                    });
                }
            }
        }

        private bool _IsInAnEmptyCell(Cell cell, Vector2 position, out Cell emptyCell)
        {
            if (cell.childrens != null)
                return _IsInAnEmptyCell(cell.childrens.item1, position, out emptyCell) || _IsInAnEmptyCell(cell.childrens.item2, position, out emptyCell);

            if (!cell.Filled && cell.Contains(position))
            {
                emptyCell = cell;
                return true;
            }
            emptyCell = null;
            return false;
        }
    }
}