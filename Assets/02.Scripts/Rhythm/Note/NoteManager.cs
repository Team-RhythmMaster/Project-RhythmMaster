using UnityEngine;
using Utils.ClassUtility;
using System.Collections.Generic;

// 오브젝트 풀링으로 Note 관리
public class NoteManager : MonoBehaviour
{
    private static NoteManager instance;
    public static NoteManager Instance { get { return instance; } }

    private Dictionary<int, Queue<NoteObject>> lanes = new();  // 각 lane별 판정 대기열
    private Dictionary<int, LongNote> activeLong = new();      // 현재 누르고 있는 longNote

    private Queue<ShortNote> shortPool = new Queue<ShortNote>();
    private Queue<LongNote> longPool = new Queue<LongNote>();

    public ShortNote shortPrefab;
    public LongNote longPrefab;

    public Transform poolParent;
    public int poolSize = 100;

    public float[] laneY = { 2f, -1f };
    public const float hitLineX = -4.5f;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        Init();
    }

    private void Init()
    {
        // lane 초기화 → lane 개수만큼 큐 생성
        for (int i = 0; i < laneY.Length; i++)
            lanes[i] = new Queue<NoteObject>();

        // 오브젝트풀 초기화
        for (int i = 0; i < poolSize; i++)
        {
            var shortNote = Instantiate(shortPrefab, poolParent);
            shortNote.gameObject.SetActive(false);
            shortPool.Enqueue(shortNote);

            var longNote = Instantiate(longPrefab, poolParent);
            longNote.gameObject.SetActive(false);
            longPool.Enqueue(longNote);
        }
    }

    // 오브젝트풀에서 Note 가져오기
    public NoteObject GetNote(NoteData _data)
    {
        NoteObject note;

        if (_data.IsLong)
            note = (longPool.Count > 0) ? longPool.Dequeue() : Instantiate(longPrefab);
        else
            note = (shortPool.Count > 0) ? shortPool.Dequeue() : Instantiate(shortPrefab);

        note.gameObject.SetActive(true);
        return note;
    }

    // Note를 오브젝트풀에 반환
    public void ReturnNote(NoteObject _note)
    {
        _note.gameObject.SetActive(false);

        if (_note is LongNote l)
            longPool.Enqueue(l);
        else if (_note is ShortNote s)
            shortPool.Enqueue(s);
    }

    // Note를 해당 lane의 판정 대기열에 등록
    public void Register(NoteObject _note)
    {
        lanes[_note.GetLane()].Enqueue(_note);
    }

    // Note를 해당 lane의 판정 대기열에서 삭제
    public void Unregister(NoteObject _note)
    {
        int lane = _note.GetLane();
        if (lanes[lane].Count > 0 && lanes[lane].Peek() == _note)
            lanes[lane].Dequeue();
    }

    // 판정 시도
    public void TryHit(int _lane)
    {
        if (lanes[_lane].Count == 0) 
            return;

        lanes[_lane].Peek().TryHit();
    }

    // longNote 누르고 있는 상태 전달
    public void Hold(int _lane, bool _holding)
    {
        if (activeLong.TryGetValue(_lane, out var ln))
            ln.SetHold(_holding);
    }

    // 활성화 longNote 추가
    public void SetActiveLongNote(int _lane, LongNote _note)
    {
        activeLong[_lane] = _note;
    }

    // 활성화 longNote 제거
    public void ClearActiveLongNote(int _lane)
    {
        activeLong.Remove(_lane);
    }

    // 모든 노트 처리가 끝났는지 확인
    public bool IsAllNotesCleared()
    {
        // 1lane 큐에 남아있는 노트 있는지 확인
        foreach (var kv in lanes)
        {
            if (kv.Value.Count > 0)
                return false;
        }

        // 롱노트 아직 눌리고 있는지 확인
        if (activeLong.Count > 0)
            return false;

        return true;
    }

    // lane 위치 반환
    public float GetLaneY(int _lane) => laneY[_lane];
}