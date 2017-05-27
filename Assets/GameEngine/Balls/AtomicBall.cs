using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEngine.Balls
{
    public class AtomicBall : Ball
    {
        public float maxYSpeed = 235f;
        public float maxXSpeed = 235f;

        private const float minAreaPercentage = 0.1f;

        public override Cell ParentCell
        {
            get { return base.ParentCell; }
            set
            {
                base.ParentCell = value;
                if (ParentCell.Area() / GameManager.Instance.RootCell.Area() < minAreaPercentage)
                {
                    StartCoroutine(_CO_Explode());
                }
            }
        }

        private void Start()
        {
            var rgbd = GetComponent<Rigidbody2D>();

            rgbd.AddForce(new Vector2(UnityEngine.Random.Range(-maxXSpeed, maxXSpeed), UnityEngine.Random.Range(maxYSpeed, 0)));
            rgbd.AddTorque(0.2f);
        }

        private IEnumerator _CO_Explode()
        {
            AtomicBallDeath anim = GetComponent<AtomicBallDeath>();
            anim.DeathAnimation();

            yield return new WaitForAtomicBallDeathAnimation(() => !anim.isOver);
            foreach (Ball b in ParentCell.Balls)
            {
                ParentCell.RemoveBall(b);
                Destroy(b.gameObject);
            }
        }
    }

    public class WaitForAtomicBallDeathAnimation : CustomYieldInstruction
    {
        Func<bool> _predicate;

        public WaitForAtomicBallDeathAnimation(Func<bool> predicate)
        {
            _predicate = predicate;
        }

        public override bool keepWaiting
        {
            get
            {
                return _predicate();
            }
        }
    }
}