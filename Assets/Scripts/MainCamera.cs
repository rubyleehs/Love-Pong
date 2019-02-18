using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour {

    public static Vector2 mousePos;
    public new static Camera camera;
    public new static Transform transform;
    public static float width;
    public static float height;

    public static Vector2 bottomLeft;
    public static Vector2 topRight;

    [Header("Positioning")]
    public float z;

    [Header("Audio")]
    public float masterVolume;

    private void Awake()
    {
        camera = GetComponent<Camera>();
        transform = GetComponent<Transform>();

        Debug.Log(camera);

        if (height > 0) camera.orthographicSize = height;
        else height = camera.orthographicSize;

        width = height * camera.aspect;
        RefreshScreenEdgePos();
    }

    private void Start()
    {
        transform.position = Vector3.forward * z;
        AudioListener.volume = masterVolume;
    }

    private void Update()
    {
        mousePos = GetMouseWorld2DPoint();
    }

    public void SetHeight(float value)
    {
        height = value;
        width = height * camera.aspect;
        camera.orthographicSize = height;
        RefreshScreenEdgePos();
    }

    public void SetPosition(Vector3 position)
    {
        z = position.z;
        transform.position = new Vector3(position.x, position.y, position.z);
        RefreshScreenEdgePos();
    }
    public void SetPosition(Vector2 position)
    {
        SetPosition(new Vector3(position.x, position.y, z));
    }

    private Vector2 GetMouseWorld2DPoint()
    {
        return camera.ScreenToWorldPoint(Input.mousePosition);
    }

    private void RefreshScreenEdgePos()
    {
        bottomLeft = camera.ScreenToWorldPoint(Vector2.zero);
        topRight = camera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }
}
