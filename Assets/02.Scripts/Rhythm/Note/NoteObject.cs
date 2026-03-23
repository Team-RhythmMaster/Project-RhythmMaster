using UnityEngine;
using System.Collections;
using Utils.ClassUtility;

// Note 데이터를 기반으로 렌더링 및 노트의 위치 계산
public abstract class NoteObject : MonoBehaviour
{
    public Note note;
    protected Conductor conductor;

    public float perfect = 0.05f;
    public float good = 0.1f;
    public float miss = 0.2f;

    private bool isHit = false;

    protected virtual void Awake()
    {
        conductor = FindFirstObjectByType<Conductor>();
    }

    protected virtual void Update()
    {
        UpdatePosition();
        CheckMiss();
    }

    // 시간 기반 위치 계산
    protected virtual void UpdatePosition()
    {
        // 현재 음악 시간
        float currentTime = conductor.songPosition;
        float y = (note.time - currentTime) * GetSpeed();
        SetVisualPosition(y);
    }

    protected abstract void SetVisualPosition(float y);

    // 초당 이동 거리
    protected virtual float GetSpeed()
    {
        return 5f;
    }

    public virtual void TryHit()
    {
        float currentTime = conductor.songPosition;
        float diff = Mathf.Abs(note.time - currentTime);

        if (diff <= perfect)
        {
            Debug.Log("Perfect");
            isHit = true;
            Destroy(gameObject);
        }
        else if (diff <= good)
        {
            Debug.Log("Good");
            isHit = true;
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Miss");
        }
    }

    protected void CheckMiss()
    {
        float currentTime = conductor.songPosition;

        if (!isHit && currentTime - note.time > miss)
        {
            Debug.Log("Miss");
            Destroy(gameObject);
        }
    }
}