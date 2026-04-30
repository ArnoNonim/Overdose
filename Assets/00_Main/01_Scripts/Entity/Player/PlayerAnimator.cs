using DG.Tweening;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class PlayerAnimator : MonoBehaviour
{
    [field: SerializeField] public PlayerInputSO PlayerInput { get; private set; }
    
    [SerializeField] private EntityMovement entityMovement;
    
    private readonly int _velocityHash = Animator.StringToHash("Velocity");
    Animator _animator;
    Camera camera;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        camera = Camera.main;
    }
    
    private void FixedUpdate()
    {
        float velocity = entityMovement.Rb.linearVelocity.magnitude;
        _animator.SetFloat(_velocityHash, velocity);

        if (PlayerInput.MoveDir.x < 0)
            gameObject.transform.DORotate(new Vector3(0, 180, 0), 0.5f).SetEase(Ease.OutBack);
        else if(PlayerInput.MoveDir.x > 0)
            gameObject.transform.DORotate(new Vector3(0, 0, 0), 0.5f).SetEase(Ease.OutBack);
    }
}
