using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TypingEffect : MonoBehaviour
{
    public TextMeshProUGUI textUI;
    public float typingSpeed = 0.05f;
    public float delayBetweenLines = 1f;
    public float fadeOutDuration = 1.5f;
    public bool typeWhenStart = false;
    public AudioSource audioSource;
    public FullTextSO fullText;

    private Coroutine typingCoroutine;
    private bool isTyping = false;
    private bool skipLine = false;
    [TextArea()] private string[] _fullText;
    
    public Action OnEndStory;

    private void Start()
    {
        if (typeWhenStart)
            StartTyping(fullText.fullText);
    }

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (isTyping)
            {
                skipLine = true;
            }
        }
    }

    public void StartTyping(string[] fullText)
    {
        _fullText = fullText;
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        textUI.alpha = 1f;
        textUI.gameObject.SetActive(true);
        typingCoroutine = StartCoroutine(TypeMultipleLines());
    }

    private IEnumerator TypeMultipleLines()
    {
        foreach (string line in _fullText)
        {
            yield return StartCoroutine(TypeSingleLine(line));

            float timer = 0f;
            while (timer < delayBetweenLines)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                    break;

                timer += Time.deltaTime;
                yield return null;
            }

            yield return StartCoroutine(FadeOutText());
            textUI.text = "";
            textUI.alpha = 1f;
        }
        OnEndStory?.Invoke();
        textUI.gameObject.SetActive(false);
    }

    private IEnumerator TypeSingleLine(string line)
    {
        textUI.text = "";
        isTyping = true;
        skipLine = false;

        foreach (char c in line)
        {
            if (skipLine)
            {
                textUI.text = line;
                break;
            }

            textUI.text += c;
            if (audioSource != null) audioSource.Play();
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        skipLine = false;
    }

    private IEnumerator FadeOutText()
    {
        float elapsed = 0f;
        float startAlpha = textUI.alpha;

        while (elapsed < fadeOutDuration)
        {
            elapsed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, 0f, elapsed / fadeOutDuration);
            textUI.alpha = newAlpha;
            yield return null;
        }

        textUI.alpha = 0f;
    }
}
