using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PongPaddle : MonoBehaviour
{
    [HideInInspector]
    public new Transform transform;
    public RectTransform imageRect;
    public TextMeshPro tmp;
    public BoxCollider2D col;

    public Image image;


    public float speed = 5;
    public float speedMultiplier = 1;

    private void Awake()
    {
        if (transform == null) transform = GetComponent<Transform>();
        if (image == null)
        {
            image = GetComponentInChildren<Image>();
            imageRect = image.GetComponent<RectTransform>();
            col = image.GetComponent<BoxCollider2D>();
        }

        if (tmp == null) tmp = GetComponentInChildren<TextMeshPro>();
        Set(tmp.text, Color.cyan);
    }

    public void Move(Vector3 dir)
    {
        if ((dir.x < 0 && transform.position.x - transform.lossyScale.x * 0.5f < MainCamera.bottomLeft.x) || (dir.x > 0 && transform.position.x + transform.lossyScale.x * 0.5f > MainCamera.topRight.x)) dir = Vector3.zero;
        transform.Translate(dir * speed * speedMultiplier * GameManager.deltaTime);
    }

    public void Set(string text, Color bgColor)
    {
        image.color = bgColor;
        tmp.text = text;
        tmp.ForceMeshUpdate();
        imageRect.sizeDelta = 2* tmp.textBounds.extents;
        col.size = 2 * tmp.textBounds.extents;
    }
}
