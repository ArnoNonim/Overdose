using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    public Image cover;
    
    public void TryStartGame()
    {
        cover.gameObject.SetActive(true);
        cover.DOColor(new Color(cover.color.r, cover.color.g, cover.color.b, 1f), 1f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            
            SceneManager.LoadScene("StoryScene");
        });
    }
}
