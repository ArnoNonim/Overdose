using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IDropHandler
{
    public GameObject correctOrb;
    public OrbPuzzle puzzle;
    public int index;
    public int expectedOrbId;
    public int currentOrbId = -1;
    public int puzzleId; // 이 슬롯의 퍼즐 ID
    
    private Image _image;
    private Color _originalColor;
    private CameraShake _cameraShake;

    void Start()
    {
        _cameraShake = Camera.main.GetComponent<CameraShake>();
        _image = GetComponent<Image>();
        _originalColor = _image.color;
    }

    public void OnDrop(PointerEventData eventData)
    {
        Orb orb = eventData.pointerDrag?.GetComponent<Orb>();
        if (orb == null) return;

        // 다른 퍼즐에서 온 오브라면 무시
        if (orb.puzzleId != this.puzzleId)
        {
            Debug.Log("다른 퍼즐의 오브입니다.");
            if(_cameraShake != null)
                _cameraShake.Play();
            else
            {
                Debug.Log("카메라쉐이크없음 조땜ㅋ");
            }
            return;
        }

        // 이전 슬롯 색 복구
        Slot prevSlot = puzzle.slots.FirstOrDefault(s => s.currentOrbId == orb.id);
        if (prevSlot != null && prevSlot != this)
        {
            prevSlot._image.DOColor(prevSlot._originalColor, 0.3f);
            prevSlot.currentOrbId = -1;
        }

        // 오브 배치
        orb.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
        currentOrbId = orb.id;

        // 정답 판단
        if (orb.id == expectedOrbId)
        {
            _image.DOColor(Color.white, 0.5f);
            GameObject orbSound = Instantiate(correctOrb, Vector3.zero, Quaternion.identity);
            orbSound.GetComponent<AudioSource>().Play();
            Destroy(orbSound, 1f);
        }
        
        else
            _image.DOColor(_originalColor, 0.5f);

        puzzle.CheckPuzzle();
    }
}


