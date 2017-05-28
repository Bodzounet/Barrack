using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEngine.Balls
{
    public class GlassBallExplosion : MonoBehaviour
    {
        public Material referenceMaterial;
        private Material _copyMaterial;

        public float dislocationStrength = 150;

        [SerializeField]
        private Rigidbody[] _rgbds;
        [SerializeField]
        private Renderer[] _renderers;

        private void Start()
        {
            _copyMaterial = new Material(referenceMaterial);

            foreach (var v in _renderers)
            {
                v.material = _copyMaterial;
            }

            StartCoroutine(_CO_Fade());
        }

        public void Explode(Vector3 explosionPoint)
        {
            foreach (var v in _rgbds)
            {
                v.AddExplosionForce(dislocationStrength, explosionPoint, 3);
            }
        }

        private IEnumerator _CO_Fade()
        {
            for (int i = 0; i < 20; i++)
            {
                _copyMaterial.SetFloat("_Opacity", 1 - (float)i / 20);
                yield return new WaitForSeconds(1f / 20);
            }
            Destroy(this.gameObject);
        }
    }
}