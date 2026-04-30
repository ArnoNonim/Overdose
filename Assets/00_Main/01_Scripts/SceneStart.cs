using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SceneStart : MonoBehaviour
{
    [SerializeField] private Image cover;
    public bool isEnding = false;

    private void Start()
    {
        if (!isEnding)
        {
            cover.color = Color.black;
            cover.DOColor(new Color(0, 0, 0, 0), 2f).SetEase(Ease.OutBack).OnComplete(() => { cover.gameObject.SetActive(false); });
        }
        else
        {
            cover.color = Color.white;
            cover.DOColor(new Color(1f, 1f, 1f, 0), 2f).SetEase(Ease.OutBack).OnComplete(() => { cover.gameObject.SetActive(false); });
        }
        
    }
}
