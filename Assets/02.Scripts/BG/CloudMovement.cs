using UnityEngine;
using DG.Tweening;

public class CloudMovement : MonoBehaviour
{
    private float speed;
    private float startX = 13f;
    private float endX = -13f;

    private void Start()
    {
        RandomReset();
    }

    private void OnEnable()
    {
        Invoke(nameof(Move), Random.Range(0f, 2f));
    }

    private void Move()
    {
        float distance = Mathf.Abs(endX - transform.position.x);
        float duration = distance / speed; // 시간 = 거리 / 속도

        transform.DOKill();
        transform.DOMoveX(endX, duration)
            .SetEase(Ease.Linear) // 일정 속도 유지
            .OnComplete(() =>
            {
                transform.position = new Vector3(startX, Random.Range(0.0f, 3.0f), 0);
                Move();
            });
    }

    public void RandomReset()
    {
        speed = Random.Range(0.6f, 1.3f);
        float scale = Random.Range(0.5f, 1.5f);
        transform.localScale = Vector3.one * scale;
    }
}
