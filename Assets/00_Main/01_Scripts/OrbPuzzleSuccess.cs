using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OrbPuzzleSuccess : MonoBehaviour
{
    [field: SerializeField] public TypingEffect TypingEffect { get; set; }
    
    [SerializeField] private Image background;
    [SerializeField] private Image cover;
    [SerializeField] private Image piece;
    [SerializeField] private ParticleSystem smokeParticles;
    [SerializeField] private FullTextSO fullText;
    
    public static OrbPuzzleSuccess Instance;

    private HashSet<int> clearedPuzzleIds = new();

    private void Start()
    {
        smokeParticles.gameObject.SetActive(false);
        background.enabled = false;
        piece.enabled = false;
    }
    
    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void MarkPuzzleCleared(int puzzleId)
    {
        if (!clearedPuzzleIds.Contains(puzzleId))
        {
            clearedPuzzleIds.Add(puzzleId);
            Debug.Log($"Puzzle {puzzleId} 완료됨. 현재 완료된 퍼즐 수: {clearedPuzzleIds.Count}");

            if (clearedPuzzleIds.Count == 3)
            {
                Debug.Log("모든 퍼즐 완료!");
                StartCoroutine(PuzzleSuccess());
            }
        }
    }
    
    public IEnumerator PuzzleSuccess()
    {
        yield return new WaitForSeconds(2f);
        PlayerPrefs.SetInt("PuzzleCleared", 2); // 진척도 저장
        PlayerPrefs.Save();
        
        background.enabled = true;
        piece.enabled = true;
        cover.gameObject.SetActive(true);
        background.DOColor(new Color32(20, 20, 20, 255), 2f).SetEase(Ease.OutBack);
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
