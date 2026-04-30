using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MoveToDream : MonoBehaviour
{
    [field: SerializeField] public TypingEffect TypingEffect { get; set; }
    [SerializeField] private Image cover;
    public bool isEnding = false;
    
    private bool _isPressed = false;

    private void Awake()
    {
        cover.color = new Color(0, 0, 0, 0);
        TypingEffect.OnEndStory += MoveToDreamScene;
    }

    private void MoveToDreamScene()
    {
        if (_isPressed == false)
        {
            _isPressed = true;
            cover.DOColor(Color.black, 2f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                if(!isEnding)
                    SceneManager.LoadScene("DreamScene");
                else
                {
                    SceneManager.LoadScene("TitleScene");
                    PlayerPrefs.SetInt("PuzzleCleared", 0);
                    PlayerPrefs.Save();
                }
            });
        }
    }

    private void OnDisable()
    {
        TypingEffect.OnEndStory -= MoveToDreamScene;
    }
}
