using UnityEngine;

public class Player : MonoBehaviour
{
    [field: SerializeField] public PlayerInputSO PlayerInput { get; private set; }
    
    #region Component region

    public EntityMovement Movement { get; private set; }

    #endregion
    
    private void Awake()
    {
        Movement = GetComponentInChildren<EntityMovement>();
    }
    
    private void Update()
    {
        SetUpMovementInput();
    }
    
    private void SetUpMovementInput()
    {
        Movement.SetMovement(PlayerInput.MoveDir);
    }
}
