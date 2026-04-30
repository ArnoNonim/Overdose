using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ToEnding : MonoBehaviour
{
    [SerializeField] private Image whiteCover;

    private void Start()
    {
        if(PlayerPrefs.GetInt("PuzzleCleared") < 3)
            gameObject.SetActive(false);
        else
            gameObject.SetActive(true);
        
        whiteCover.gameObject.SetActive(false);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            whiteCover.gameObject.SetActive(true);
            whiteCover.DOColor(Color.white, 4f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                PlayerPrefs.SetInt("PuzzleCleared", 4);
                PlayerPrefs.Save();
                SceneManager.LoadScene("EndingScene");
            });
        }
    }
}
