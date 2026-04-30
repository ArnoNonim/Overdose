using System.Collections;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class ProgressManager : MonoBehaviour
{
    [SerializeField] private GameObject piece1;
    [SerializeField] private GameObject piece2;
    [SerializeField] private GameObject piece3;
    [SerializeField] private Image cover;
    [SerializeField] private Tilemap prevTilemap;
    [SerializeField] private Tilemap nextTilemap;

    private int _pressedTimes = 0;

    private void Start()
    {
        nextTilemap.color = new Color(1f, 1f, 1f, 0);
        nextTilemap.gameObject.SetActive(false);
        piece2.SetActive(false);
        piece3.SetActive(false);
        cover.color = Color.black;
        cover.DOColor(new Color(0, 0, 0, 0), 2f).SetEase(Ease.OutBack).OnComplete(() => { cover.gameObject.SetActive(false); });
        int puzzleCleared = PlayerPrefs.GetInt("PuzzleCleared");
        if (puzzleCleared == 1)
        {
            piece1.SetActive(false);
            piece2.SetActive(true);
            piece3.SetActive(false);
        }
        if (puzzleCleared == 2)
        {
            piece1.SetActive(false);
            piece2.SetActive(false);
            piece3.SetActive(true);
            StartCoroutine(ChangeTiles());
        }

        if (puzzleCleared == 3)
        {
            piece1.SetActive(false);
            piece2.SetActive(false);
            piece3.SetActive(false);
            nextTilemap.gameObject.SetActive(true);
            nextTilemap.color = Color.white;
            prevTilemap.gameObject.SetActive(false);
            
        }
    }

    private IEnumerator ChangeTiles()
    {
        nextTilemap.gameObject.SetActive(true);
        yield return new WaitForSeconds(4);
        DOTween.To(
            () => prevTilemap.color,
            x => {
                prevTilemap.color = x;
            },
            new Color(1f, 1f, 1f, 0),
            2f
        ).SetEase(Ease.OutBack).OnComplete(() =>
        {
            DOTween.To(
                () => nextTilemap.color,
                x =>
                {
                    nextTilemap.color = x;
                },
                new Color(1f, 1f, 1f, 1f),
                2f
            ).SetEase(Ease.OutBack);
            prevTilemap.gameObject.SetActive(false); 
        });
    }
    
    private void Update()
    {
        if (Keyboard.current.ctrlKey.wasPressedThisFrame)
        {
            _pressedTimes++;
            if (_pressedTimes == 10)
            {
                Debug.Log("PuzzleCleared is now 0.");
                PlayerPrefs.SetInt("PuzzleCleared", 0);
                PlayerPrefs.Save();
            }
            else if (_pressedTimes == 11)
            {
                Debug.Log("PuzzleCleared is now 1.");
                PlayerPrefs.SetInt("PuzzleCleared", 1);
                PlayerPrefs.Save();
            }
            else if (_pressedTimes == 12)
            {
                Debug.Log("PuzzleCleared is now 2.");
                PlayerPrefs.SetInt("PuzzleCleared", 2);
                PlayerPrefs.Save();
            }
            else if (_pressedTimes == 13)
            {
                Debug.Log("PuzzleCleared is now 3.");
                PlayerPrefs.SetInt("PuzzleCleared", 3);
                PlayerPrefs.Save();
            }
            else if (_pressedTimes == 14)
            {
                Debug.Log("PuzzleCleared is now 4.");
                PlayerPrefs.SetInt("PuzzleCleared", 4);
                PlayerPrefs.Save();
            }
        }
    }
}
