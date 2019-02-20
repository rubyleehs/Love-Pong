using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatScreen : MonoBehaviour
{
    public static RectTransform rect;

    public RectTransform canvasTransform;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(canvasTransform.sizeDelta.x, canvasTransform.sizeDelta.y);
    }
}
