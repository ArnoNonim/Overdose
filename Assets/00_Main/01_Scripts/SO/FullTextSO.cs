using UnityEngine;

[CreateAssetMenu(fileName = "FullText", menuName = "SO/FullText")]
public class FullTextSO : ScriptableObject
{
    [TextArea()] public string[] fullText;
}
