using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OrbPuzzle : MonoBehaviour
{
    public int slotCount = 4;
    public int puzzleId; // 퍼즐 고유 ID
    public GameObject orbPrefab;
    public GameObject slotPrefab;
    public RectTransform puzzleParent;

    [HideInInspector] public List<Slot> slots = new();
    private List<Orb> orbs = new();

    void Start()
    {
        GeneratePuzzle();
    }

    void GeneratePuzzle()
    {
        slots.Clear();
        orbs.Clear();

        // 슬롯 개수만큼 ID 리스트 생성 및 섞기
        List<int> ids = Enumerable.Range(0, slotCount).ToList();
        List<int> shuffled = ids.OrderBy(x => Random.value).ToList();

        float radius = 200f; // 원형 반지름
        Vector2 center = Vector2.zero; // 부모 기준 중심 (RectTransform 기준)

        // 1. 슬롯 먼저 전부 생성
        for (int i = 0; i < slotCount; i++)
        {
            float angle = (360f / slotCount) * i;
            float rad = angle * Mathf.Deg2Rad;
            Vector2 slotPos = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * radius;

            GameObject slotGO = Instantiate(slotPrefab, puzzleParent);
            RectTransform slotRect = slotGO.GetComponent<RectTransform>();
            slotRect.anchoredPosition = slotPos;

            Slot slot = slotGO.GetComponent<Slot>();
            slot.expectedOrbId = shuffled[i];
            slot.puzzleId = puzzleId;
            slot.puzzle = this;
            slots.Add(slot);
        }

// 2. 슬롯이 다 만들어졌으니, 오브를 랜덤한 위치에 생성
        float orbRadius = radius * 0.7f;
        for (int i = 0; i < slotCount; i++)
        {
            Vector2 orbPos = Random.insideUnitCircle * orbRadius;

            GameObject orbGO = Instantiate(orbPrefab, puzzleParent);
            RectTransform orbRect = orbGO.GetComponent<RectTransform>();
            orbRect.anchoredPosition = orbPos;

            Orb orb = orbGO.GetComponent<Orb>();
            orb.Init(shuffled[i], this); // 여기서 puzzle 전달
            orbs.Add(orb);

        }
    }

    public void CheckPuzzle()
    {
        foreach (var slot in slots)
        {
            if (slot.currentOrbId != slot.expectedOrbId)
                return;
        }
        OrbPuzzleSuccess.Instance.MarkPuzzleCleared(puzzleId);
    }
}
