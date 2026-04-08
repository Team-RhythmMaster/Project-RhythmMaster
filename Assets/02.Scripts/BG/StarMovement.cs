using DG.Tweening;
using UnityEngine;

public class StarMovement : MonoBehaviour
{
    private Vector2 direction = new Vector2(-1, -1);
    private float speed;

    private float screenLeft;
    private float screenRight;
    private float screenTop;
    private float screenBottom;

    private void Start()
    {
        Camera cam = Camera.main;
        Vector3 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 topRight = cam.ViewportToWorldPoint(new Vector3(1, 1, 0));

        screenLeft = bottomLeft.x;
        screenRight = topRight.x;
        screenBottom = bottomLeft.y;
        screenTop = topRight.y;
    }

    private void OnEnable()
    {
        Invoke(nameof(Move), Random.Range(0f, 2f));
    }

    private void Move()
    {
        RandomReset();

        // bottom까지 얼마나 이동해야 하는지 계산하고 방향 벡터 기준으로 실제 이동 거리 보정
        float distance = (transform.position.y - screenBottom) / Mathf.Abs(direction.y);
        float duration = distance / speed;

        transform.DOKill();
        transform.DOMove(transform.position + (Vector3)(direction * distance), duration)
            .SetEase(Ease.Linear)
            .OnComplete(Move);
    }

    public void RandomReset()
    {
        float randomX = Random.Range(screenLeft + 5.0f, screenRight);
        float offsetY = Random.Range(0f, 2f);

        speed = Random.Range(3.0f, 5.0f);
        transform.position = new Vector2(randomX, screenTop + offsetY);
    }
}
