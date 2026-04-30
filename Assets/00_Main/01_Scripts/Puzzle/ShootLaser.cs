using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShootLaser : MonoBehaviour
{
    [field: SerializeField] public CameraShake cameraShake { get; set; }
    [field: SerializeField] public TypingEffect typingEffect { get; set; }
    
    [SerializeField] private GameObject laserShooter;
    [SerializeField] private GameObject laser;
    [SerializeField] private Material laserMaterial;
    [SerializeField] private GameObject hitbox;
    [SerializeField] private GameObject victimLaser;
    [SerializeField] private Image background;
    [SerializeField] private Image cover;
    [SerializeField] private Image piece;
    [SerializeField] private ParticleSystem smokeParticles;
    [SerializeField] private FullTextSO fullText;
    
    [Header("Settings")]
    [SerializeField] private float defDistanceRay = 100;

    public Transform laserFirePos;
    public LineRenderer lineRenderer;

    private Vector3 _firstLaserPos;
    private ReflectLaser _reflectLaser;
    
    private bool _isSucceed = false;
    private bool _isRest = false;
    private void Start()
    {
        smokeParticles.gameObject.SetActive(false);
        background.enabled = false;
        piece.enabled = false;
        lineRenderer.enabled = false;
        _firstLaserPos = laser.transform.position;
        StartCoroutine(Appear());
    }

    private void Update()
    {
        if(!_isRest)
            LaserShoot();
    }

    private IEnumerator Appear()
    {
        if (!_isSucceed)
        {
            yield return new WaitForSeconds(2f);

            // 레이저 발사 시작
            laser.transform.DOLocalMoveX(0.72f, 2f)
                .SetEase(Ease.InSine)
                .OnStart(() =>
                {
                    _isRest = false; // 레이저 이동 중에는 아직 궤적 그리지 않음
                    lineRenderer.enabled = false;
                })
                .OnComplete(() =>
                {
                    //이동이 끝난 후 궤적 그리기 시작
                    lineRenderer.enabled = true;
                    _isRest = true;

                    laserMaterial.DOColor(new Color(1f, 1f, 1f, 0), 2f).OnStart(() =>
                        {
                            _isRest = false; // 사라지는 동안 쏘기 멈춤
                        })
                        .OnComplete(() =>
                        {
                            lineRenderer.enabled = false;
                            laserMaterial.color = Color.white;
                            laser.transform.position = _firstLaserPos;
                            StartCoroutine(Appear());
                        });

                    cameraShake.Play();
                });
        }
    }


    private bool hasTriggeredSuccess = false;

    private void LaserShoot()
    {
        Vector2 startPos = laserFirePos.position;
        Vector2 direction = transform.right;
        int maxReflections = 6;
        float maxRayDistance = defDistanceRay;

        List<Vector3> laserPoints = new List<Vector3> { startPos };

        for (int i = 0; i < maxReflections + 1; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(startPos, direction, maxRayDistance);

            if (hit)
            {
                laserPoints.Add(hit.point);

                // ✅ 특정 태그에 맞았는지 검사
                if (hit.collider.CompareTag("Glitch") && !hasTriggeredSuccess)
                {
                    hasTriggeredSuccess = true;
                    OnSuccess(hit.point);
                }

                ReflectLaser reflect = hit.collider.GetComponent<ReflectLaser>();
                if (reflect != null)
                {
                    direction = reflect.GetNewDirection(direction);
                    startPos = hit.point + direction * 0.05f;
                }
                else
                {
                    break;
                }
            }
            else
            {
                laserPoints.Add(startPos + direction * maxRayDistance);
                break;
            }
        }

        lineRenderer.positionCount = laserPoints.Count;
        for (int i = 0; i < laserPoints.Count; i++)
        {
            lineRenderer.SetPosition(i, laserPoints[i]);
        }
    }
    
    private void OnSuccess(Vector3 finalHitPoint)
    {
        Debug.Log("Laser ended successfully at: " + finalHitPoint);
        PlayerPrefs.SetInt("PuzzleCleared", 1); // 진척도 저장
        PlayerPrefs.Save();
        
        victimLaser.transform.DOLocalMoveX(-0.574f, 0);
        victimLaser.transform.DOLocalMoveX(-0.3f, 2f).SetEase(Ease.InSine).OnComplete(() =>
        {
            background.enabled = true;
            piece.enabled = true;
            cover.gameObject.SetActive(true);
            background.DOColor(new Color32(20, 20, 20, 255), 2f).SetEase(Ease.OutBack);
            piece.DOColor(new Color(1, 1, 1, 1f), 2f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                smokeParticles.gameObject.SetActive(true);
                typingEffect.StartTyping(fullText.fullText);
                StartCoroutine(End());
            });
        });
    }

    private IEnumerator End()
    {
        yield return new WaitForSeconds(4f);
        cover.DOColor(new Color(0, 0, 0, 1f), 2f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            SceneManager.LoadScene("StoryScene");
        });
    }
}
