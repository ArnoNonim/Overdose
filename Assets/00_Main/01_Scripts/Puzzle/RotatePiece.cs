using DG.Tweening;
using UnityEngine;

public class RotatePiece : MonoBehaviour
{
    private void Start()
    {
        transform.DORotate(new Vector3(0, 360, 0), 3f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1);
    }
}
