using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PongBall : MonoBehaviour
{
    [HideInInspector]
    public SpriteRenderer spriteRenderer;
    [HideInInspector]
    public new Transform transform;
    [HideInInspector]
    public new Collider2D collider;
    [HideInInspector]
    public PongManager pongManager;
    public float radius;
    
    public int id;
    [HideInInspector]
    public string qString;

    public float speed = 3;
    public Vector2 moveDir = Vector2.one;
    public float initialFadeDelay;
    public float fadeDur;

    public float desummonDuration;
    public float desummonScaleRatio;


    [Header("Runtime Values")]
    private float initialSpeed;
    private Color initialColor;
    private Color fadeColor;
    [HideInInspector]
    public Vector3 initialScale;
    private float timeSinceLastCollision;

    private void Awake()
    {
        if (transform == null) transform = GetComponent<Transform>();
        if(spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        if (collider == null) collider = GetComponent<Collider2D>();
        Debug.Log(collider);
        radius = transform.lossyScale.x * 0.5f;

        moveDir = moveDir.normalized;
        timeSinceLastCollision = -initialFadeDelay;
    }

    private void Start()
    {
        SetCurrentAsInitialSetting();
    }

    private void SetCurrentAsInitialSetting()
    {
        initialSpeed = speed;
        initialScale = transform.localScale;
        initialColor = spriteRenderer.color;

        fadeColor = initialColor;
        fadeColor.a = 0;
    }

    public void RevertToInitialSetting()
    {
        speed = initialSpeed;
        transform.localScale = transform.localScale;
        spriteRenderer.color = initialColor;
        fadeColor = initialColor;
        fadeColor.a = 0;

        spriteRenderer.enabled = true;
        collider.enabled = true;
        gameObject.SetActive(true);
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
            StartCoroutine(Desummon(true));
        }
        else if(transform.position.y > MainCamera.topRight.y)
        {
            StartCoroutine(pongManager.Answer(this));
        }
    }

    void Fade()
    {
        spriteRenderer.color = Color.Lerp(initialColor, fadeColor, Mathf.SmoothStep(0, 1, timeSinceLastCollision / fadeDur));
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (timeSinceLastCollision > 0) timeSinceLastCollision = 0;

        if (collision.transform.CompareTag("Paddle"))
        {
            moveDir = (transform.position - collision.transform.position).normalized;
            collision.transform.parent.parent.GetComponent<PongPaddle>().Set(qString, initialColor);
            speed *= 1.085f;
        }
        else
        {
            float avgSpeed = (collision.transform.GetComponent<PongBall>().speed + speed) * 0.5f;
            speed = avgSpeed;
            moveDir = Vector2.Reflect(moveDir, collision.GetContact(0).normal);
        }

        moveDir = moveDir.normalized;
    }

    public IEnumerator Desummon(bool showAnimation)
    {
        collider.enabled = false;
        if (showAnimation)
        {
            float timeElapsed = 0;
            float smoothProgress = 0;

            Vector3 startScale = transform.localScale;
            Vector3 endScale = startScale * desummonScaleRatio;
            Color startColor = spriteRenderer.color;

            while (smoothProgress < 1)
            {
                timeElapsed += GameManager.deltaTime;
                smoothProgress = Mathf.SmoothStep(0, 1, timeElapsed / desummonDuration);

                speed = Mathf.Lerp(speed, 0, smoothProgress);
                transform.localScale = Vector3.Lerp(startScale, endScale, smoothProgress);
                if (smoothProgress < 0.5f) spriteRenderer.color = Color.Lerp(startColor, Color.Lerp(initialColor, fadeColor, 0.5f), smoothProgress * 2);
                else spriteRenderer.color = Color.Lerp(Color.Lerp(initialColor, fadeColor, 0.5f), fadeColor, smoothProgress * 2);
                yield return null;
            }
        }

        this.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }
}
