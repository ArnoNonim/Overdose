using DG.Tweening;
using TMPro;
using UnityEngine;

public class Interact : MonoBehaviour
{
    [field: SerializeField] public PlayerInputSO PlayerInput { get; set; }
    [field:SerializeField] public CameraZoomInEffect CameraZoomInEffect { get; set; }
    
    public TMP_Text text;

    [Header("Settings")] 
    [SerializeField] private bool useCameraEffect = false;

    private Color _firstColor;
    private bool _isInteractable = false;

    private void OnEnable()
    {
        PlayerInput.OnInteractKeyPressed += InteractObject;
    }
    
    private void Awake()
    {
        _firstColor = text.color;
        text.color = new Color(_firstColor.r, _firstColor.g, _firstColor.b, 0);
    }

    private void InteractObject()
    {
        if (_isInteractable)
        {
            Debug.Log("Interacted");
            CameraZoomInEffect.PlayEffect(gameObject);
            _isInteractable = false;
            text.DOColor(new Color(_firstColor.r, _firstColor.g, _firstColor.b, 0), 1f).SetEase(Ease.OutBack);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _isInteractable = true;
            text.DOColor(new Color(_firstColor.r, _firstColor.g, _firstColor.b, 1f), 1f).SetEase(Ease.OutBack);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _isInteractable = false;
            text.DOColor(new Color(_firstColor.r, _firstColor.g, _firstColor.b, 0), 1f).SetEase(Ease.OutBack);
        }
    }

    private void OnDisable()
    {
        PlayerInput.OnInteractKeyPressed -= InteractObject;
    }
}
