using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class OptionsButton : MonoBehaviour
{
    public Canvas optionsCanvas;
    public Image cover;

    private void Start()
    {
        optionsCanvas.gameObject.SetActive(false);
    }
    
    public void TryOpenOptions()
    {
        cover.gameObject.SetActive(true);
        cover.DOColor(new Color(cover.color.r, cover.color.g, cover.color.b, 1f), 1f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            optionsCanvas.gameObject.SetActive(true);
            cover.DOColor(new Color(cover.color.r, cover.color.g, cover.color.b, 0), 1f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                cover.gameObject.SetActive(false);
            });
        });
    }
}
