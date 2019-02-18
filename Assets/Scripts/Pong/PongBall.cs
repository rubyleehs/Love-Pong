using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PongBall : MonoBehaviour
{
    public new Transform transform;
    public int index;

    public float speed = 3;
    public Vector2 moveDir = Vector2.one;
    public float initialFadeDelay;
    public float fadeDur;

    private SpriteRenderer spriteRenderer;
    private float radius;
    private Color initialColor;
    private Color fadeColor;
    private float timeSinceLastCollision;//

    private void Awake()
    {
        if (transform == null) transform = GetComponent<Transform>();
        if(spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        radius = transform.lossyScale.x * 0.5f;

        moveDir = moveDir.normalized;
        timeSinceLastCollision = -initialFadeDelay;
        initialColor = spriteRenderer.color;
        fadeColor = initialColor;
        fadeColor.a = 0;
    }

    private void Update()
    {
        timeSinceLastCollision += GameManager.deltaTime;
        Move(GameManager.deltaTime);
        Fade();
    }

    public void Move(float dt)
    {
        transform.Translate(moveDir * speed * GameManager.deltaTime);

        if ((transform.position.x - radius < MainCamera.bottomLeft.x && moveDir.x < 0) || (transform.position.x + radius > MainCamera.topRight.x && moveDir.x > 0))
        {
            if (timeSinceLastCollision > 0) timeSinceLastCollision = 0;
            moveDir.x *= -1;
        }

        if (transform.position.y < MainCamera.bottomLeft.y)
        {
            //GameOver
        }
        else if(transform.position.y > MainCamera.topRight.y)
        {
            //Next senario, pass index
        }
    }

    void Fade()
    {
        spriteRenderer.color = Color.Lerp(initialColor, fadeColor, Mathf.SmoothStep(0, 1, timeSinceLastCollision / fadeDur));
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (timeSinceLastCollision > 0) timeSinceLastCollision = 0;

        if (collision.transform.CompareTag("Paddle")) moveDir = (transform.position - collision.transform.position).normalized;
        else moveDir = Vector2.Reflect(moveDir, collision.GetContact(0).normal);

    }
}
