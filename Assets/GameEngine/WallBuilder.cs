﻿using System;
using UnityEngine;

namespace GameEngine
{
    public class WallBuilder : MonoBehaviour
    {
        public const float WallWidth = 0.1f;

        [SerializeField]
        private Ray[] halfrays;

        private Action _onComplete;
        private Action _onFail;

        private void Awake()
        {
            halfrays[0].OnHit += _CB_OnHalfRayIsHit;
            halfrays[1].OnHit += _CB_OnHalfRayIsHit;

            halfrays[0].OnReachAWall += _CB_OnHalfRayIsOver;
            halfrays[1].OnReachAWall += _CB_OnHalfRayIsOver;
        }

        public void DrawRay(Vector3 startingPos, RayLauncher.RayDirection direction, Action onComplete, Action onFail)
        {
            _onComplete = onComplete;
            _onFail = onFail;
            if (direction == RayLauncher.RayDirection.Horizontal)
            {
                halfrays[0].DrawRay(startingPos, Ray.RayDirection.Left);
                halfrays[1].DrawRay(startingPos, Ray.RayDirection.Right);
            }
            else
            {
                halfrays[0].DrawRay(startingPos, Ray.RayDirection.Up);
                halfrays[1].DrawRay(startingPos, Ray.RayDirection.Down);
            }
        }

        private void _CB_OnHalfRayIsOver()
        {
            if (halfrays[0].isOver && halfrays[1].isOver)
            {
                halfrays[0].OnHit -= _CB_OnHalfRayIsHit;
                halfrays[1].OnHit -= _CB_OnHalfRayIsHit;
                Destroy(halfrays[0]);
                Destroy(halfrays[1]);

                halfrays[0].gameObject.layer = LayerMask.NameToLayer("Wall");
                halfrays[1].gameObject.layer = LayerMask.NameToLayer("Wall");

                if (_onComplete != null)
                    _onComplete();
            }
        }

        private void _CB_OnHalfRayIsHit()
        {
            if (_onFail != null)
                _onFail();
            Destroy(this.gameObject);
        }
    }
}