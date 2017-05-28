using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEngine.Balls
{
    public class GlassBallExplosion : MonoBehaviour
    {
        public void AddImpulse(Vector3 pos)
        {
            GetComponent<Rigidbody>().AddExplosionForce(10, pos, 3);
        }
    }
}