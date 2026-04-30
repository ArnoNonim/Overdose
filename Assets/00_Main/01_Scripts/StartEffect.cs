using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class StartEffect : MonoBehaviour
{
    public Image cover;
    public Light2D light;
    
    private void Start()
    {
        Color fullColor = cover.color;
        fullColor.a = 1f;
        cover.color = fullColor;
        StartCoroutine(ShowTitle());
        
    }

    private IEnumerator ShowTitle()
    {
        yield return new WaitForSeconds(2f);
        cover.DOColor(new Color(cover.color.r, cover.color.g, cover.color.b, 0), 3f).SetEase(Ease.OutBack).OnComplete(() => cover.gameObject.SetActive(false));
        DOTween.To(
            () => light.intensity,
            x => light.intensity = x,
            0.5f,
            2f
        ).SetEase(Ease.OutBack);
        DOTween.To(
            () => light.falloffIntensity,
            x => light.falloffIntensity = x,
            0.5f,
            2f
        ).SetEase(Ease.OutBack);
    }
}
