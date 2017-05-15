using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ray : MonoBehaviour
{
    [SerializeField]
    private HalfRay[] halfrays;

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
            halfrays[0].DrawRay(startingPos, HalfRay.RayDirection.Left);
            halfrays[1].DrawRay(startingPos, HalfRay.RayDirection.Right);
        }
        else
        {
            halfrays[0].DrawRay(startingPos, HalfRay.RayDirection.Up);
            halfrays[1].DrawRay(startingPos, HalfRay.RayDirection.Down);
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
