using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStat", menuName = "SO/PlayerStat")]
public class PlayerStatSO : ScriptableObject
{
    [Header("Health")]
    public int maxHealth = 6;
    public int curHealth;
    
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;

    [Header("Speed")]
    public float moveSpeed = 3;
}
