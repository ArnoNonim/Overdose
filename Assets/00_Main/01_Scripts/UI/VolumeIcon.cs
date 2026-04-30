using UnityEngine;
using UnityEngine.UI;

public class VolumeIcon : MonoBehaviour
{
    public Sprite volume3;
    public Sprite volume2;
    public Sprite volume1;
    public Sprite volume0;
    public Slider volumeSlider;
    
    private Image volumeIcon;

    private void Awake()
    {
        volumeIcon = GetComponent<Image>();
        
        volumeSlider.onValueChanged.AddListener(OnValueChanged);
    }

    public void OnValueChanged(float value)
    {
        if(value >= 0.9f)
            volumeIcon.sprite = volume3;
        else if (value >= 0.6f)
            volumeIcon.sprite = volume2;
        else if (value >= 0.3f || value > 0.01f)
            volumeIcon.sprite = volume1;
        else if (value <= 0.01f)
            volumeIcon.sprite = volume0;
    }
}
