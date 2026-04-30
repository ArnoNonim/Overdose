using UnityEngine;

public class StoryProgress : MonoBehaviour
{
    [field: SerializeField] public TypingEffect TypingEffect { get; set; }
    [SerializeField] private FullTextSO startText;
    [SerializeField] private FullTextSO fullText1;
    [SerializeField] private FullTextSO fullText2;
    [SerializeField] private FullTextSO fullText3;
    [SerializeField] private FullTextSO endingText;
    private void Start()
    {
        if (PlayerPrefs.GetInt("PuzzleCleared") == 0)
            TypingEffect.fullText = startText;
        else if(PlayerPrefs.GetInt("PuzzleCleared") == 1)
            TypingEffect.fullText = fullText1;
        else if (PlayerPrefs.GetInt("PuzzleCleared") == 2)
            TypingEffect.fullText = fullText2;
        else if (PlayerPrefs.GetInt("PuzzleCleared") == 3)
            TypingEffect.fullText = fullText3;
        else if(PlayerPrefs.GetInt("PuzzleCleared") == 4)
            TypingEffect.fullText = endingText;
    }
}
