using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FirstStoryFlow : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private TypingEffect typingEffect;
    [TextArea] public string[] fullText;

    private void Start()
    {
        StartCoroutine(StartStory());
    }

    private IEnumerator StartStory()
    {
        yield return new WaitForSeconds(5f);
    }
}
