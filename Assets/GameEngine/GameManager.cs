using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject root;
    public Transform[] rootLimits; // bottomLeft, topRight

    public GameObject[] balls; // PawnBall

    private Cell _rootCell;
    public Cell RootCell
    {
        get { return _rootCell; }
    }

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        _rootCell = gameObject.AddComponent<Cell>();
        _rootCell.Ctor(rootLimits[0].position, rootLimits[1].position);

        GameObject ball = Instantiate(balls[0], new Vector3(Random.Range(RootCell.bottomLeft.x, RootCell.topRight.x), Random.Range(RootCell.bottomLeft.y, RootCell.topRight.y)), Quaternion.identity);
        _rootCell.AddBall(ball.GetComponent<Ball>());
    }
}
