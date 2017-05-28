using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEngine.Balls
{
    public class GlassBall : Ball
    {
        public Texture[] ballTextures; // many cracks, little cracks, uncracked
        public GameObject P_BrokenBall;

        public float maxSpeed = 50;

        private Renderer _renderer;

        private Vector3 collisionPoint;

        private int _life;
        private int Life
        {
            get { return _life; }
            set
            {
                if (Life > 3 || Life < 0)
                    throw new ArgumentException("This ball can only have a number of life between 0 and 3");

                _life = value;
                if (Life > 0)
                {
                    _renderer.material.SetTexture("_Normal", ballTextures[Life - 1]);
                }
                else
                {
                    _DestroyBall();
                }
            }
        }

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
        }

        private void Start()
        {
            Life = 3;
            GetComponent<Rigidbody2D>().AddForce(new Vector2(UnityEngine.Random.Range(-maxSpeed, maxSpeed), UnityEngine.Random.Range(-maxSpeed, maxSpeed)));
            GetComponent<Rigidbody2D>().AddTorque(UnityEngine.Random.Range(0f, 1f));
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (((1 << collision.gameObject.layer) & LayerMask.GetMask("Ray", "GlassBall")) > 0)
            {
                collisionPoint = collision.contacts[0].point;
                Life--;
            }
        }

        private void _DestroyBall()
        {
            GameObject brokenBall = GameObject.Instantiate(P_BrokenBall, transform.position, Quaternion.identity);
            brokenBall.GetComponent<GlassBallExplosion>().Explode(collisionPoint);
            ParentCell.RemoveBall(this);
            Destroy(this.gameObject);
        }
    }
}