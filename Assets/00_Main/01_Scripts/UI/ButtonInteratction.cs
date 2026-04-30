using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ButtonInteraction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float scaleUp = 1.1f;
    public float duration = 0.2f;
    public AudioSource audioSource;
    public ParticleSystem particleSystem;
    
    
    private Material _particleMaterial;
    private Color _particleColor;
    private Vector3 _originalScale;

    void Start()
    {
        _originalScale = transform.localScale;
        _particleMaterial = particleSystem.GetComponent<Renderer>().material;
        _particleColor = _particleMaterial.GetColor("_EmissionColor");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOScale(_originalScale * scaleUp, duration).SetEase(Ease.OutBack);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOScale(_originalScale, duration).SetEase(Ease.OutBack);
    }

    public void OnClick()
    {
        audioSource.Play();
        
        DOTween.KillAll();
        ChangeParticle();
        
        transform.DOScale(_originalScale * (scaleUp + 0.1f), duration).SetEase(Ease.OutBack).OnComplete(() =>
        {
            // 클릭 후 hover 상태로 돌아가기 (커서가 올라간 상태 가정)
            transform.DOScale(_originalScale * scaleUp, duration * 4).SetEase(Ease.OutBack);
        });;
    }

    private void ChangeParticle()
    {
        _particleMaterial.SetColor("_EmissionColor", _particleColor * Mathf.LinearToGammaSpace(30));
        var main = particleSystem.main;
        main.simulationSpeed = 2f;
        float currentSpeed = main.simulationSpeed;
        DOTween.To(
            () => currentSpeed,
            (float x) => {
                currentSpeed = x;
                main.simulationSpeed = x;
            },
            1f,
            1f
        );
        _particleMaterial.DOColor(_particleColor, "_EmissionColor", 2).SetEase(Ease.OutBack);
    }
}