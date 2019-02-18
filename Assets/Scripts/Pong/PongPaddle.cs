using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PongPaddle : MonoBehaviour
{
    [HideInInspector]
    public new Transform transform;

    public float speed = 5;
    public float speedMultiplier = 1;

    private void Awake()
    {
        if (transform == null) transform = GetComponent<Transform>();
    }

    public void Move(Vector3 dir)
    {
        if ((dir.x < 0 && transform.position.x - transform.lossyScale.x * 0.5f < MainCamera.bottomLeft.x) || (dir.x > 0 && transform.position.x + transform.lossyScale.x * 0.5f > MainCamera.topRight.x)) dir = Vector3.zero;
        transform.Translate(dir * speed * speedMultiplier * GameManager.deltaTime);
    }
}
