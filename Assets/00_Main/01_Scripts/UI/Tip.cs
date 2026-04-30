using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tip : MonoBehaviour
{
    public bool showWhenStart = false;
    public float waitTime = 1f;
    public float showDuration = 2f;
    public float fadeOutDuration = 4f;
    
    private TMP_Text text;

    private void Start()
    {
        text = GetComponent<TMP_Text>();
        text.color = new Color(1f, 1f, 1f, 0);
        
        if (showWhenStart)
            StartCoroutine(ShowTip(waitTime));
    }

    private IEnumerator ShowTip(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        text.DOColor(Color.white, showDuration).SetEase(Ease.OutBack).OnComplete(() =>
        {
            text.DOColor(new Color(1f, 1f, 1f, 0), fadeOutDuration).SetEase(Ease.OutBack);
        });
    }
}
