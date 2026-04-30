using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Orb : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject orbPickUp;
    public GameObject OrbDrop;
    
    public int id;              // 고유 오브 ID
    public int puzzleId = -1; 
    public OrbPuzzle puzzle;

    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private RectTransform puzzleParent;
    private Canvas canvas;

    public void Init(int orbId, OrbPuzzle puzzleRef)
    {
        this.id = orbId;
        this.puzzle = puzzleRef;
        this.puzzleId = puzzleRef.puzzleId;
        this.puzzleParent = puzzleRef.puzzleParent;
    }


    
    void Start()
    {
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        
        if (canvas.renderMode == RenderMode.WorldSpace && canvas.worldCamera == null)
        {
            Debug.LogWarning("World Space Canvas인데 worldCamera가 지정되지 않았습니다.");
        }

        if (puzzle != null)
            puzzleParent = puzzle.puzzleParent;
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        GameObject orbSound = Instantiate(orbPickUp, Vector3.zero, Quaternion.identity);
        orbSound.GetComponent<AudioSource>().Play();
        Destroy(orbSound, 1f);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canvas.renderMode == RenderMode.WorldSpace)
        {
            // World Space Canvas인 경우, 직접 월드 좌표를 계산해서 RectTransform 위치 이동
            Vector3 worldPos;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                puzzleParent, 
                eventData.position, 
                canvas.worldCamera, 
                out worldPos
            );
            rectTransform.position = worldPos;
        }
        else
        {
            // Screen Space (Overlay or Camera) 캔버스용 기존 처리
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                puzzleParent, 
                eventData.position,
                canvas.renderMode == RenderMode.ScreenSpaceCamera ? canvas.worldCamera : null,
                out localPoint
            );
            rectTransform.anchoredPosition = localPoint;
        }
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        GameObject orbSound = Instantiate(OrbDrop, Vector3.zero, Quaternion.identity);
        orbSound.GetComponent<AudioSource>().Play();
        Destroy(orbSound, 1f);
    }
}