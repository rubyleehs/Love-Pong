using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[ExecuteInEditMode]
public class SpeechBubble : MonoBehaviour
{
    public new RectTransform transform;
    public Image bgImage;
    public TextMeshProUGUI tmp;

    public float lineHeight = 1.7f;
    public Vector2 paddingX = new Vector2(0.6f, 0.6f);
    public Vector2 paddingY = new Vector2(0.3f, 0.3f);
    [HideInInspector]
    public float height;

    public Vector2 displacement;

    private void Awake()
    {
        if (transform == null) transform = GetComponent<RectTransform>();
        if (bgImage == null) bgImage = GetComponentInChildren<Image>();
        if (tmp == null) tmp = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        UpdateText(tmp.text);
    }

    private void UpdateText(string text)
    {
        tmp.text = text;
        //Debug.Log(tmp.text.Split('\n').Length);
        //float height = tmp.text.Split('\n').Length * tmp.fontSize * (tmp.lineSpacing + lineHeight) + (paddingY.x + paddingY.y);
        tmp.ForceMeshUpdate();
        bgImage.rectTransform.sizeDelta = tmp.textBounds.extents * 2 + new Vector3(paddingX.x + paddingX.y, paddingY.x + paddingY.y);
        transform.sizeDelta = tmp.textBounds.extents * 2 + new Vector3(paddingX.x + paddingX.y, paddingY.x + paddingY.y);
        tmp.transform.localPosition = displacement + new Vector2(-paddingX.y + transform.sizeDelta.x * 0.5f, - paddingY.y + transform.sizeDelta.y * 0.5f);
    }
}
