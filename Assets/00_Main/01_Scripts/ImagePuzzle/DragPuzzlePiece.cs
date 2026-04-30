using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragPuzzlePiece : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Vector2Int CorrectGridPos { get; private set; }
    public RectTransform Rect { get; private set; }
    private DragPuzzleManager manager;
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    public bool IsSnapped { get; private set; } = false; // ✅ 고정 여부

    private float snapThreshold => Mathf.Min(manager.pieceWidth, manager.pieceHeight) * 0.4f;

    public void Init(Vector2Int correctPos, DragPuzzleManager mgr)
    {
        CorrectGridPos = correctPos;
        manager = mgr;
        Rect = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void SetImage(Sprite sprite)
    {
        GetComponent<Image>().sprite = sprite;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (IsSnapped) return;

        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.8f;
        Rect.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (IsSnapped) return;

        Rect.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (IsSnapped) return;

        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        Vector2 correctPos = manager.GetSlotPosition(CorrectGridPos);

        if (Vector2.Distance(Rect.anchoredPosition, correctPos) < snapThreshold)
        {
            Rect.anchoredPosition = correctPos;
            IsSnapped = true;

            //맨 아래로 보내기 (정답 맞춘 조각은 배경처럼 뒤로 감)
            Rect.SetSiblingIndex(0);
        }

        manager.CheckPuzzleCompletion(); //스냅 여부로 완성 판정
    }
}