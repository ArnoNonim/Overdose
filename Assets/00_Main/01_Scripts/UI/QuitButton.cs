using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class QuitButton : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public Image line;
    public Light2D light;
    public Button startButton;
    public Button optionsButton;
    public Image cover;
    public Image particleCover;
    public ParticleSystem particleSystem;
    public AudioSource backgroundMusic;
    
    
    public void TryQuit()
    {
        particleCover.DOColor(new Color(particleCover.color.r, particleCover.color.g, particleCover.color.b, 1f), 3f).SetEase(Ease.OutBack);
        DOTween.To(
            () => light.intensity,
            x => light.intensity = x,
            0,
            2f
        ).SetEase(Ease.OutBack);
        var main = particleSystem.main;
        main.simulationSpeed = 2f;
        float currentSpeed = main.simulationSpeed;
        DOTween.To(
            () => currentSpeed,
            (float x) => {
                currentSpeed = x;
                main.simulationSpeed = x;
            },
            0,
            1f
        );
        ChangeText();
        MoveButton();
    }

    private void ChangeText()
    {
        Color color = titleText.color;
        titleText.DOColor(new Color(color.r, color.g, color.b, 0f), 1f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            titleText.text = "GOOD BYE";
            
        });
        line.DOColor(new Color(color.r, color.g, color.b, 0f), 1f).SetEase(Ease.OutBack);
    }

    private void MoveButton()
    {
        transform.GetComponent<FloatingUI>().enabled = false;
        transform.GetComponent<ButtonInteraction>().enabled = false;
        gameObject.GetComponent<Button>().interactable = false;
        
        startButton.GetComponent<FloatingUI>().enabled = false;
        startButton.GetComponent<ButtonInteraction>().enabled = false;
        startButton.GetComponent<Button>().interactable = false;
        
        optionsButton.GetComponent<FloatingUI>().enabled = false;
        optionsButton.GetComponent<ButtonInteraction>().enabled = false;
        optionsButton.GetComponent<Button>().interactable = false;

        transform.DOMoveY(transform.position.y - 1, 2f).SetEase(Ease.OutQuart);
        GetComponentInChildren<TextMeshProUGUI>().DOColor(new Color(1f, 1f, 1f, 0), 0.5f).SetEase(Ease.OutQuart).OnComplete(() =>
        {
            optionsButton.transform.DOMoveY(optionsButton.transform.position.y - 1, 2f).SetEase(Ease.OutQuart);
            optionsButton.GetComponentInChildren<TextMeshProUGUI>().DOColor(new Color(1f, 1f, 1f, 0), 0.5f).SetEase(Ease.OutQuart).OnComplete(() =>
            {
                startButton.transform.DOMoveY(startButton.transform.position.y - 1, 2f).SetEase(Ease.OutQuart);
                startButton.GetComponentInChildren<TextMeshProUGUI>().DOColor(new Color(1f, 1f, 1f, 0), 0.5f).SetEase(Ease.OutQuart).OnComplete(() =>
                {
                    StartCoroutine(Quit());
                });
            });
        });
    }

    private IEnumerator Quit()
    {
        titleText.gameObject.transform.DOLocalMoveY(0, 1f).SetEase(Ease.OutBack);
        titleText.gameObject.GetComponent<FloatingUI>().enabled = false;
        titleText.DOColor(new Color(1f, 1f, 1f, 1f), 1f).SetEase(Ease.OutBack);
        yield return new WaitForSeconds(1f);
        cover.gameObject.SetActive(true);
        cover.DOColor(new Color32(20, 20, 20, 255), 2f).SetEase(Ease.InQuart);
        DOTween.To(
            () => backgroundMusic.volume,
            (float x) => {
                backgroundMusic.volume = x;
            },
            0,
            2f
        );
        yield return new WaitForSeconds(2f);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
