using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PongManager : MonoBehaviour
{
    [Header("Refrences")]
    public PongBall ballPrefab;
    public PongPaddle paddlePrefab;
    public Transform pongParent;

    [Header("Stats")]
    public float paddlePadding;

    private List<PongBall> balls = new List<PongBall>();
    private PongPaddle playerPaddle;
    private PongPaddle enemyPaddle;

    // Start is called before the first frame update
    void Start()
    {
        SpawnBall();
        SetupPaddles();

    }

    // Update is called once per frame
    void Update()
    {
        HandlePlayerInput();
        ControlEnemyPaddle();
    }

    public void SpawnBall()
    {
        balls.Add(Instantiate(ballPrefab));
    }

    public void SetupPaddles()
    {
        if (playerPaddle == null) playerPaddle = Instantiate(paddlePrefab,  Vector2.up * (MainCamera.bottomLeft.y + paddlePadding), Quaternion.identity, pongParent);
        if (enemyPaddle == null) enemyPaddle = Instantiate(paddlePrefab,  Vector2.up * (MainCamera.topRight.y - paddlePadding), Quaternion.identity, pongParent);
    }

    public void HandlePlayerInput()
    {
        playerPaddle.Move(Vector3.right * GameManager.inputAxis.x);
    }

    public void ControlEnemyPaddle()
    {
        int targetIndex = 0;
        for (int i = 1; i < balls.Count; i++)
        {
            if(Vector3.SqrMagnitude(enemyPaddle.transform.position - balls[targetIndex].transform.position) < Vector3.SqrMagnitude(enemyPaddle.transform.position - balls[i].transform.position))
            {
                targetIndex = i;
            }
        }

        PongBall target = balls[targetIndex];
        if (target != null)
        {
            enemyPaddle.Move(Vector3.right * (target.transform.position - enemyPaddle.transform.position).x);
        }
    }

}
