using UnityEngine;
using DG.Tweening;

public class CloudMovement : MonoBehaviour
{
    private float speed = 1f;
    private float randomSpeed;
    private float resetX = -13f;  // 구름이 화면 밖으로 나갔을 때 도착할 위치
    private float startX = 13f;   // 다시 시작할 위치 (오른쪽 바깥)

    private void OnEnable()
    {
        Move();
    }

    private void Move()
    {
        RandomSetting();

        // 현재 위치에서 resetX까지의 거리 계산
        float distance = Mathf.Abs(resetX - transform.position.x);
        // 거리 / 속도 = 시간
        float duration = distance / randomSpeed;

        transform.DOKill();
        transform.DOMoveX(resetX, duration)
            .SetEase(Ease.Linear) // 일정 속도 유지
            .OnComplete(() =>
            {
                transform.position = new Vector3(startX, Random.Range(0f, 3f), 0);
                Move();
            });
    }

    private void RandomSetting()
    {
        float scale = Random.Range(0.5f, 1.3f);
        transform.localScale = Vector3.one * scale;
        randomSpeed = speed * Random.Range(0.6f, 1.3f);
    }
}
