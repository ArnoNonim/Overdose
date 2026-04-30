using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CameraZoomInEffect : MonoBehaviour
{
    public Transform target;           // 빨려 들어갈 대상
    public float zoomDuration = 1.5f;  // 연출 시간
    public float targetSize = 2f;      // 카메라 OrthographicSize (2D일 경우)
    public float moveOffset = 0f;      // 약간 더 가까이 붙고 싶으면
    public Image cover;

    private SceneChange _sceneChange;
    private Camera _cam;
    private float _originalSize;
    private Vector3 _originalPos;

    private void Start()
    {
        _cam = Camera.main;
        _originalSize = _cam.orthographicSize;
        _originalPos = _cam.transform.position;
    }

    public void PlayEffect(GameObject interactedObject)
    {
        Vector3 targetPos = target.position;
        targetPos.z = _cam.transform.position.z + moveOffset;

        _cam.transform.DOMove(targetPos, zoomDuration).SetEase(Ease.InOutSine);
        cover.gameObject.SetActive(true);
        cover.DOColor(Color.black, zoomDuration).SetEase(Ease.OutBack);
        _cam.DOOrthoSize(targetSize, zoomDuration).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            DOTween.KillAll();
            interactedObject.GetComponent<SceneChange>().ChangeScene();
        });
    }

    public void ResetCamera()
    {
        _cam.transform.DOMove(_originalPos, zoomDuration).SetEase(Ease.InOutSine);
        _cam.DOOrthoSize(_originalSize, zoomDuration).SetEase(Ease.InOutSine);
    }
}