using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static Dialogue.ChatManager chatManager;
    public static PongManager pongManager;

    public static float deltaTime;
    public static float fixedDeltaTime;
    public static float timeScale = 1;

    public static Vector2 inputAxis;

    private void Awake()
    {
        if (chatManager == null) chatManager = GetComponent<Dialogue.ChatManager>();
        if (pongManager == null) pongManager = GetComponent<PongManager>();
    }

    private void Update()
    {
        deltaTime = Time.deltaTime * timeScale;

        inputAxis = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    private void FixedUpdate()
    {
        fixedDeltaTime = Time.fixedDeltaTime * timeScale;
    }
}
