using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEngine
{
    public class Shark : MonoBehaviour
    {
        public float targetingError = 0.5f;
        public float marginBetweenSharkAndGameBorder = 0.5f;

        public float initialAttackPercentageChanceAfterAttack = 30;
        public float increaseAttackPercentageChanceAfterRoaming = 10;

        public float enrageFactor = 3;

        private int roamingCount = 0;

        private Transform model;
        private Vector3 modelRotation;

        private bool _isActive = false;

        [SerializeField]
        private float _speed;
        public float Speed
        {
            get
            {
                float remainingDistanceToDestination = Vector2.Distance(transform.position, _Destination);
                return _speed / 2 * (1 +  Mathf.Min(remainingDistanceToDestination, 2) / 2);
            }
        }

        [SerializeField]
        private float _rotationSpeed;
        public float RotationSpeed
        {
            get { return _rotationSpeed; }
        }

        private Vector3 _destination;
        private Vector3 _Destination
        {
            get { return _destination; }
            set
            {
                _destination = _ClampV2InGameArea(value);
            }
        }

        private void Start()
        {
            model = transform.root.GetChild(0).transform;
            modelRotation = model.rotation.eulerAngles;
            //Invoke("_Spawn", Random.Range(30, 90));
            _Spawn();
        }

        public void Die()
        {
            if (!_isActive)
                return;
            Destroy(this.transform.root.gameObject);
        }

        private void _Spawn()
        {
            this.GetComponent<Collider2D>().enabled = true;
            model.GetComponent<Renderer>().enabled = true;
            _MoveToTarget();
            Invoke("_Enrage", Random.Range(5, 60));

            _isActive = true;
        }

        private void _Enrage()
        {
            StartCoroutine(_CO_Enrage());
        }

        private void _MoveToTarget()
        {
            _Destination = new Vector3(GameManager.Instance.RayLauncher.transform.position.x, GameManager.Instance.RayLauncher.transform.position.y, transform.position.z)
                + new Vector3(Random.Range(-targetingError, targetingError), Random.Range(-targetingError, targetingError), 0);
            StartCoroutine(_CO_Move(_Destination));
        }

        private void _Roam()
        {
            Cell root = GameManager.Instance.RootCell;
            _Destination = new Vector3(Random.Range(root.bottomLeft.x, root.topRight.x), Random.Range(root.bottomLeft.y, root.topRight.y), transform.position.z);
            StartCoroutine(_CO_Move(_Destination));
        }

        private IEnumerator _CO_Move(Vector3 targetPos)
        {
            while (true)
            {
                transform.root.position += transform.right * Speed * Time.deltaTime;

                Vector3 vectorToTarget = targetPos - transform.position;
                float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
                Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);

                transform.rotation = Quaternion.Slerp(transform.rotation, q, RotationSpeed * Time.deltaTime);
                model.rotation = Quaternion.Euler(modelRotation + new Vector3( 0, 0, -transform.rotation.eulerAngles.z));

                yield return new WaitForEndOfFrame();
                if (Vector3.Distance(transform.position, targetPos) < 1f)
                    break;
            }

            if (Random.Range(0, 100) > 100 - (initialAttackPercentageChanceAfterAttack + roamingCount * increaseAttackPercentageChanceAfterRoaming))
            {
                roamingCount = 0;
                _MoveToTarget();
            }
            else
            {
                roamingCount++;
                _Roam();
            }
        }

        private IEnumerator _CO_Enrage()
        {
            float originalSpeed = Speed;
            float originalRotationSpeed = RotationSpeed;

            _speed *= enrageFactor;
            _rotationSpeed *= enrageFactor;

            yield return new WaitForSeconds(Random.Range(15, 45));

            _speed = originalSpeed;
            _rotationSpeed = originalRotationSpeed;
            Invoke("_Enrage", Random.Range(5, 60));
        }

        private Vector3 _ClampV2InGameArea(Vector3 v)
        {
            Cell root = GameManager.Instance.RootCell;
            return new Vector3(Mathf.Clamp(v.x, root.bottomLeft.x + marginBetweenSharkAndGameBorder, root.topRight.x - marginBetweenSharkAndGameBorder),
                Mathf.Clamp(v.y, root.bottomLeft.y + marginBetweenSharkAndGameBorder, root.topRight.y - marginBetweenSharkAndGameBorder),
                v.z);
        }
    }
}