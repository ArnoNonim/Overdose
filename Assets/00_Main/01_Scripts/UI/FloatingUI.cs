using UnityEngine;

public class FloatingUI : MonoBehaviour
{
    public float amplitude = 10f; // 이동 높이
    public float frequency = 1f;  // 속도

    private RectTransform rectTransform;
    private Vector2 startPos;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startPos = rectTransform.anchoredPosition;
    }

    void Update()
    {
        float newY = Mathf.Sin(Time.time * frequency) * amplitude;
        rectTransform.anchoredPosition = startPos + new Vector2(0f, newY);
    }
}