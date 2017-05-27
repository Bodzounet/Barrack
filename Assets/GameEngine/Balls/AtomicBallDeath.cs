using System.Collections;
using UnityEngine;

namespace GameEngine.Balls
{
    public class AtomicBallDeath : MonoBehaviour
    {
        [SerializeField]
        ParticleSystem _lightings;
        [SerializeField]
        ParticleSystem _sparks;
        [SerializeField]
        ParticleSystem _smoke;

        [SerializeField]
        GameObject _model;

        public bool isOver
        {
            get;
            private set;
        }

        private void Start()
        {
            isOver = false;
        }

        public void DeathAnimation()
        {
            StartCoroutine(_CO_DeathAnimation());
        }

        private IEnumerator _CO_DeathAnimation()
        {
            Destroy(GetComponent<Rigidbody2D>());
            Destroy(GetComponent<Collider2D>());

            _lightings.Play();
            Vector3 startScale = _model.transform.localScale;
            for (int i = 0; i < 20; i++)
            {
                _model.transform.localScale = Vector3.Lerp(startScale, Vector3.zero, i / 20f);
                yield return new WaitForSeconds(1f / 20);
            }
            Destroy(_model);

            _sparks.Play();
            _smoke.Play();
            yield return new WaitForSeconds(2);
            isOver = true;
        }
    }
}