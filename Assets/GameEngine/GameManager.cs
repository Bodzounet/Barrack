using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEngine
{
    public class GameManager : MonoBehaviour
    {
        public GameObject root;
        public Transform[] rootLimits; // bottomLeft, topRight

        public GameObject[] P_balls; // PawnBall
        public GameObject P_shark;

        [SerializeField]
        private RayLauncher _rayLauncher;
        public RayLauncher RayLauncher
        {
            get { return _rayLauncher; }
        }

        [SerializeField]
        private ForeGroundPainter _foregroundPainter;
        public ForeGroundPainter ForegroundPainter
        {
            get { return _foregroundPainter; }
        }

        public Cell RootCell
        {
            get;
            private set;
        }

        public Shark Shark
        {
            get;
            private set;
        }

        [SerializeField]
        private GameProgression _gameProgression;
        public GameProgression Progression
        {
            get { return _gameProgression; }
        }

        private static GameManager _instance = null;
        public static GameManager Instance
        {
            get { return _instance; }
        }

        protected virtual void Awake()
        {
            DontDestroyOnLoad(gameObject);

            if (_instance == null)
            {
                _instance = this;
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            Progression.OnCoveredAreaIsModified += (float percentageArea) =>
            {
                Debug.Log(percentageArea);
            };
            StartGame();
        }

        public void StartGame()
        {
            RootCell = new Cell(rootLimits[0].position, rootLimits[1].position);

            var v = (RootCell.bottomLeft + RootCell.topRight) / 2;
            Shark = Instantiate(P_shark, new Vector3(v.x, v.y, -2), Quaternion.identity).GetComponentInChildren<Shark>();

            //GameObject ball = Instantiate(P_balls[0], new Vector3(Random.Range(RootCell.bottomLeft.x, RootCell.topRight.x), Random.Range(RootCell.bottomLeft.y, RootCell.topRight.y), -1), Quaternion.identity);
            //GameObject ball2 = Instantiate(P_balls[1], new Vector3(Random.Range(RootCell.bottomLeft.x, RootCell.topRight.x), Random.Range(RootCell.bottomLeft.y, RootCell.topRight.y) -1), P_balls[1].transform.rotation);
            //GameObject ball = Instantiate(P_balls[2], new Vector3(Random.Range(RootCell.bottomLeft.x, RootCell.topRight.x), Random.Range(RootCell.bottomLeft.y, RootCell.topRight.y), -1), Quaternion.identity);
            //RootCell.AddBall(ball.GetComponent<Ball>());
            //RootCell.AddBall(ball2.GetComponent<Ball>());

            for (int i = 0; i < 10; i++)
            {
                GameObject ball = Instantiate(P_balls[2], new Vector3(Random.Range(RootCell.bottomLeft.x, RootCell.topRight.x), Random.Range(RootCell.bottomLeft.y, RootCell.topRight.y), -1), Quaternion.identity);
                RootCell.AddBall(ball.GetComponent<Ball>());
            }
        }
    }
}