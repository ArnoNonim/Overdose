using UnityEngine;

public class ReflectLaser : MonoBehaviour
{
    public bool reflect = true; // false면 굴절

    public Vector2 GetNewDirection(Vector2 inDir)
    {
        if (reflect)
        {
            // 표면 노멀 기준 반사
            Vector2 normal = transform.up; // 예: 오브젝트의 up 방향
            return Vector2.Reflect(inDir, normal).normalized;
        }
        else
        {
            // 굴절이라면 원하는 방향 지정
            return transform.right.normalized; // 예: 오른쪽으로 굴절
        }
    }
}
