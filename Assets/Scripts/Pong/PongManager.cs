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

    [Header("Animation")]
    public float absorbBallMoveSpeed = 3;


    // Start is called before the first frame update
    void Start()
    {
        SetupPaddles();
    }

    // Update is called once per frame
    void Update()
    {
        HandlePlayerInput();
        ControlEnemyPaddle();
    }

    public void SpawnBall(int id, Color color)
    {
        PongBall b = Instantiate(ballPrefab);
        b.id = id;
        b.GetComponent<SpriteRenderer>().color = color;
        b.pongManager = this;
        balls.Add(b);
    }

    public void SetupPaddles()
    {
        if (playerPaddle == null) playerPaddle = Instantiate(paddlePrefab,  Vector2.up * (MainCamera.bottomLeft.y + paddlePadding), Quaternion.identity, pongParent);
        if (enemyPaddle == null) enemyPaddle = Instantiate(paddlePrefab,  Vector2.up * (MainCamera.topRight.y - paddlePadding), Quaternion.identity, pongParent);

        //playerPaddle.transform.position = Vector2.up * (MainCamera.bottomLeft.y + paddlePadding);
    }

    public void HandlePlayerInput()
    {
        playerPaddle.Move(Vector3.right * GameManager.inputAxis.x);
    }

    public void ControlEnemyPaddle()
    {
        if (balls.Count == 0) return;
        int targetIndex = 0;
        
        for (int i = 1; i < balls.Count; i++)
        {
            if(balls[i] == null)
            {
                balls.RemoveAt(i);
                i--;
            }

            if(balls[i].transform.position.y > balls[targetIndex].transform.position.y)
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

    public IEnumerator Answer(PongBall ball)
    {
        if (!GameManager.chatManager.isExpectingAnswer) yield break;
        GameManager.chatManager.isExpectingAnswer = false;

        for (int i = 0; i < balls.Count; i++)
        {
            //if (balls[i] == ball) continue;
            StartCoroutine(balls[i].Desummon(true));
        }
        /*
        float timeElapsed = 0;
        float smoothProgress = 0;
        Vector3 startPos = ball.transform.position;
        ball.RevertToInitialSetting();
        ball.collider.enabled = false;

        while (smoothProgress < 1)
        {
            timeElapsed += GameManager.deltaTime;
            smoothProgress = Mathf.SmoothStep(0, 1, timeElapsed / (ball.fadeDur));

            ball.transform.position = Vector3.Lerp(startPos, playerPaddle.transform.position, smoothProgress);
            ball.transform.localScale = Vector3.Lerp(ball.initialScale, Vector3.zero, smoothProgress);
            yield return null;
        }
        
        StartCoroutine(ball.Desummon(false));
        */
        GameManager.chatManager.Answer(ball.id);
    }

}
