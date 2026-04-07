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
        Vector3 camPos = cam.transform.position;

        float height = cam.orthographicSize;
        float width = height * cam.aspect;

        screenLeft = camPos.x - width;
        screenRight = camPos.x + width;
        screenTop = camPos.y + height;
        screenBottom = camPos.y - height;
    }

    private void OnEnable()
    {
        Invoke(nameof(StartFall), Random.Range(0f, 2f));
    }

    private void StartFall()
    {
        ResetPosition();
        Move();
    }

    private void Move()
    {
        speed = Random.Range(2.5f, 5.0f);
        Vector2 dir = direction.normalized;

        // 충분히 멀리 이동
        float distance = (screenRight - screenLeft);
        float duration = distance / speed;

        transform.DOMove(transform.position + (Vector3)(dir * distance), duration)
            .SetEase(Ease.Linear)
            .OnComplete(StartFall);
    }

    private void ResetPosition()
    {
        float randomX = Random.Range(screenLeft + 5, screenRight + 5);
        float offsetY = Random.Range(0f, 2f);

        transform.position = new Vector2(randomX, screenTop + offsetY);
    }
}
