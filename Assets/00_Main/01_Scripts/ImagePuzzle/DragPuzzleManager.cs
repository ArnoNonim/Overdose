using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class DragPuzzleManager : MonoBehaviour
{
    [field: SerializeField] public TypingEffect TypingEffect { get; set; }
    
    public int rows = 3;
    public int columns = 3;
    public Sprite sourceImage;
    public Image door;
    public Image eyes;
    public GameObject piecePrefab;
    public RectTransform puzzleArea;
    public AudioSource backgroundMusic;
    public AudioSource tinnitus;
    [SerializeField] private Image background;
    [SerializeField] private Image cover;
    [SerializeField] private Image piece;
    [SerializeField] private ParticleSystem smokeParticles;
    [SerializeField] private FullTextSO fullText;

    public float pieceWidth { get; private set; }
    public float pieceHeight { get; private set; }
    private List<DragPuzzlePiece> pieces = new();

    private void Start()
    {
        smokeParticles.gameObject.SetActive(false);
        background.enabled = false;
        piece.enabled = false;
        door.color = new Color(1f, 1f, 1f, 0);
        eyes.color = new Color(1f, 1f, 1f, 0);
        tinnitus.volume = 0;
        GeneratePuzzle();
    }

    private void GeneratePuzzle()
    {
        pieceWidth = puzzleArea.rect.width / columns;
        pieceHeight = puzzleArea.rect.height / rows;

        Texture2D texture = sourceImage.texture;
        int texWidth = texture.width / columns;
        int texHeight = texture.height / rows;

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                GameObject pieceObj = Instantiate(piecePrefab, puzzleArea);
                RectTransform rect = pieceObj.GetComponent<RectTransform>();
                rect.sizeDelta = new Vector2(pieceWidth, pieceHeight);

                DragPuzzlePiece piece = pieceObj.GetComponent<DragPuzzlePiece>();
                piece.Init(new Vector2Int(x, y), this);

                // 이미지 자르기
                Sprite part = Sprite.Create(texture,
                    new Rect(x * texWidth, texture.height - (y + 1) * texHeight, texWidth, texHeight),
                    new Vector2(0.5f, 0.5f));
                piece.SetImage(part);

                // 랜덤 위치 배치
                float randX = Random.Range(-puzzleArea.rect.width / 2, puzzleArea.rect.width / 2);
                float randY = Random.Range(-puzzleArea.rect.height / 2, puzzleArea.rect.height / 2);
                rect.anchoredPosition = new Vector2(randX, randY);

                pieces.Add(piece);
            }
        }
    }

    public Vector2 GetSlotPosition(Vector2Int gridPos)
    {
        float x = (gridPos.x + 0.5f) * pieceWidth - puzzleArea.rect.width / 2f;
        float y = -(gridPos.y + 0.5f) * pieceHeight + puzzleArea.rect.height / 2f;
        return new Vector2(x, y);
    }



    public bool IsCorrect(DragPuzzlePiece piece)
    {
        Vector2 correctPos = GetSlotPosition(piece.CorrectGridPos);
        Debug.Log("짱");
        return Vector2.Distance(piece.Rect.anchoredPosition, correctPos) < pieceWidth * 0.1f;
    }

    public void CheckPuzzleCompletion()
    {
        foreach (var piece in pieces)
        {
            if (!piece.IsSnapped)
                return;
        }

        Debug.Log("🎉 퍼즐 완성!");
        PlayerPrefs.SetInt("PuzzleCleared", 3); // 진척도 저장
        PlayerPrefs.Save();
        StartCoroutine(Horror());

    }

    private IEnumerator Horror()
    {
        yield return new WaitForSeconds(1f);
        backgroundMusic.Stop();
        yield return new WaitForSeconds(2f);
        GameObject doorImage = Instantiate(door.gameObject, puzzleArea);
        doorImage.GetComponent<Image>().color = Color.white;
        yield return new WaitForSeconds(1f);
        tinnitus.Play();
        DOTween.To(
            () => tinnitus.volume,
            (float x) => {
                tinnitus.volume = x;
            },
            0.2f,
            1f
        );
        GameObject eyeImage = Instantiate(eyes.gameObject, puzzleArea);
        eyeImage.GetComponent<Image>().DOColor(Color.white, 1f).SetEase(Ease.Linear).OnComplete(() =>
        {
            StartCoroutine(DelayedReveal());
        });
    }

    private IEnumerator DelayedReveal()
    {
        yield return new WaitForSeconds(5f);
        DOTween.To(
            () => tinnitus.volume,
            (float x) => {
                tinnitus.volume = x;
            },
            0,
            1f
        );
        background.enabled = true;
        piece.enabled = true;
        cover.gameObject.SetActive(true);
        background.DOColor(new Color32(200, 200, 200, 255), 2f).SetEase(Ease.OutBack);
        piece.DOColor(new Color(1, 1, 1, 1f), 2f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            smokeParticles.gameObject.SetActive(true);
            TypingEffect.StartTyping(fullText.fullText);
            StartCoroutine(End());
        });
    }

    private IEnumerator End()
    {
        yield return new WaitForSeconds(4f);
        cover.DOColor(new Color(0, 0, 0, 1f), 2f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            SceneManager.LoadScene("StoryScene");
        });
    }
}
