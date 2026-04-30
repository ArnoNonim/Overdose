using UnityEngine;
using UnityEngine.InputSystem;

public class EntityMovement : MonoBehaviour
{
    [field: SerializeField] public PlayerStatSO PlayerStat { get; private set; }

    protected Vector2 _move;
    
    public void SetMovement(Vector2 move) => _move = move;
    
    [field: SerializeField] public Rigidbody2D Rb { get; private set; }
    
    private void FixedUpdate()
    {
        MoveEntity();
    }
    
    private void MoveEntity()
    {
        Rb.linearVelocity = _move * PlayerStat.moveSpeed;
    }
}
