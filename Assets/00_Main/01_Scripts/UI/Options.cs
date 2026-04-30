using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    public Canvas optionsCanvas;
    public AudioMixer mainMixer;
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    public Image cover;

    private void Start()
    {
        // 슬라이더 초기값을 PlayerPrefs에서 불러오기
        float master = PlayerPrefs.GetFloat("MasterVolume", 1f);
        float music = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float sfx = PlayerPrefs.GetFloat("SFXVolume", 1f);

        masterVolumeSlider.value = master;
        musicVolumeSlider.value = music;
        sfxVolumeSlider.value = sfx;

        SetMasterVolume(master);
        SetMusicVolume(music);
        SetSFXVolume(sfx);

        // 슬라이더가 변경될 때 볼륨 반영 + 저장
        masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetMasterVolume(float value)
    {
        mainMixer.SetFloat("Master", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("MasterVolume", value);
    }

    public void SetMusicVolume(float value)
    {
        mainMixer.SetFloat("Music", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    public void SetSFXVolume(float value)
    {
        mainMixer.SetFloat("SFX", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    public void OnReturnToMainMenu()
    {
        cover.gameObject.SetActive(true);
        cover.DOColor(new Color(cover.color.r, cover.color.g, cover.color.b, 1f), 1f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            optionsCanvas.gameObject.SetActive(false);
            cover.DOColor(new Color(cover.color.r, cover.color.g, cover.color.b, 0), 1f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                cover.gameObject.SetActive(false);
            });
        });
    }
}
