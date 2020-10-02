using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TooltipUI : MonoBehaviour
{
    public static TooltipUI Instance {get; private set;}
    [SerializeField] private RectTransform canvasRectTransform;
    private RectTransform rectTransform;
    private TextMeshProUGUI textMeshPro;
    private RectTransform backgorundRectTransform;
    private ToolTipTimer toolTipTimer;
    private void Awake()
    {
        Instance = this;
        rectTransform = GetComponent<RectTransform>();
        textMeshPro = transform.Find("text").GetComponent<TextMeshProUGUI>();
        backgorundRectTransform = transform.Find("background").GetComponent<RectTransform>();
        Hide();
    }
    private void Update()
    {
        HandleFollowMouse();
        if (toolTipTimer != null)
        {
            toolTipTimer.timer -= Time.deltaTime;
            if (toolTipTimer.timer <= 0)
            {
                Hide();
            }
        }
    }

    private void HandleFollowMouse()
    {
        Vector2 anchoredPosition = rectTransform.anchoredPosition = Input.mousePosition / canvasRectTransform.localScale.x;
        if (anchoredPosition.x + backgorundRectTransform.rect.width > canvasRectTransform.rect.width)
        {
            anchoredPosition.x = canvasRectTransform.rect.width - backgorundRectTransform.rect.width;
        }
        if (anchoredPosition.y + backgorundRectTransform.rect.height > canvasRectTransform.rect.height)
        {
            anchoredPosition.y = canvasRectTransform.rect.height - backgorundRectTransform.rect.height;
        }
        rectTransform.anchoredPosition = anchoredPosition;
    }

    private void SetText(string tooltipText)
    {
        textMeshPro.SetText(tooltipText);
        textMeshPro.ForceMeshUpdate();
        Vector2 textSize = textMeshPro.GetRenderedValues(false);
        Vector2 padding = new Vector2(8,8);
        backgorundRectTransform.sizeDelta = textSize + padding;
    }
    public void Show(string tooltipText,ToolTipTimer toolTipTimer = null)
    {
        this.toolTipTimer = toolTipTimer;
        gameObject.SetActive(true);
        SetText(tooltipText);
        HandleFollowMouse();
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public class ToolTipTimer{
        public float timer;
    }
}
