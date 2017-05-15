using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnBall : Ball
{
    public float maxSpeed = 235f;

    private void Start()
    {
        GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-maxSpeed, maxSpeed), Random.Range(-maxSpeed, maxSpeed)));
    }
}
