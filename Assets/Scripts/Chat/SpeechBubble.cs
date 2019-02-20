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

    public RectTransform positioner;
    public Vector2 playerPosition;
    public Vector2 playerDisplacement;
    public Vector2 otherPosition;
    public Vector2 otherDisplacement;

    public Color playerColor;
    public Color otherColor;

    public float lineHeight = 1.7f;
    public Vector2 paddingX = new Vector2(0.6f, 0.6f);
    public Vector2 paddingY = new Vector2(0.3f, 0.3f);
    [HideInInspector]
    public float height;

    public Vector2 displacement;

    //[HideInInspector]
    public bool isPlayerSpeech = true;

    private int alignment;

    private void Awake()
    {
        if (transform == null) transform = GetComponent<RectTransform>();
        if (bgImage == null) bgImage = GetComponentInChildren<Image>();
        if (tmp == null) tmp = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        SetCharacter(isPlayerSpeech);
        SetText(tmp.text);
    }


    private void Update()
    {
        SetCharacter(isPlayerSpeech);
        SetText(tmp.text);
    }
    

    public void SetText(string text)
    {
        tmp.text = text;
        //Debug.Log(tmp.text.Split('\n').Length);
        //float height = tmp.text.Split('\n').Length * tmp.fontSize * (tmp.lineSpacing + lineHeight) + (paddingY.x + paddingY.y);
        tmp.ForceMeshUpdate();
        bgImage.rectTransform.sizeDelta = tmp.textBounds.extents * 2 + new Vector3(paddingX.x + paddingX.y, paddingY.x + paddingY.y);
        bgImage.rectTransform.localPosition = new Vector3(tmp.textBounds.extents.x * -alignment, tmp.textBounds.extents.y) + new Vector3((paddingX.x + paddingX.y) * -alignment * 0.5f, (paddingY.x + paddingY.y) * 0.5f);
        transform.sizeDelta = tmp.textBounds.extents * 2 + new Vector3(paddingX.x + paddingX.y, paddingY.x + paddingY.y);
        tmp.rectTransform.localPosition = displacement + Vector2.right * (tmp.textBounds.extents.x + 0.5f * (paddingX.x - paddingX.y)) * alignment + Vector2.up * (paddingY.x - paddingY.y) * 0.5f;
    }

    public void SetCharacter(bool _isPlayerSpeech)
    {
        isPlayerSpeech = _isPlayerSpeech;

        if (isPlayerSpeech)
        {
            bgImage.color = playerColor;
            tmp.alignment = TextAlignmentOptions.Right;
            displacement = playerDisplacement;
            playerPosition = Vector3.right * MainCamera.topRight.x;
            positioner.localPosition = Vector3.right * MainCamera.width *0.5f;
            alignment = 1;
        }
        else
        {
            bgImage.color = otherColor;
            tmp.alignment = TextAlignmentOptions.Left;
            displacement = otherDisplacement;
            positioner.localPosition = otherPosition;
            alignment = -1;
        }
    }
}
