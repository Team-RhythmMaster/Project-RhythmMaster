using UnityEngine;
using System.Collections.Generic;

public class NoteManager : MonoBehaviour
{
    private static NoteManager instance;
    public static NoteManager Instance { get { return instance; } }

    // Lane별 큐 (판정용)
    private Dictionary<int, Queue<NoteObject>> notesByLane = new Dictionary<int, Queue<NoteObject>>();
    private Dictionary<int, LongNote> activeLongNotes = new Dictionary<int, LongNote>();

    // 풀링 (타입별 분리)
    private Queue<ShortNote> shortPool = new Queue<ShortNote>();
    private Queue<LongNote> longPool = new Queue<LongNote>();

    public ShortNote shortPrefab;
    public LongNote longPrefab;

    public Transform poolParent;
    public int poolSize = 100;

    private Vector3 spawnPos = new Vector3(10.0f, 0.0f, 0.0f);
    public float[] laneY = { 1f, -1f };

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

        // lane 초기화
        for (int i = 0; i < laneY.Length; i++)
        {
            notesByLane[i] = new Queue<NoteObject>();
        }

        Init();
    }

    void Init()
    {
        for (int i = 0; i < poolSize; i++)
        {
            ShortNote s = Instantiate(shortPrefab, poolParent);
            s.gameObject.SetActive(false);
            shortPool.Enqueue(s);

            LongNote l = Instantiate(longPrefab, poolParent);
            l.gameObject.SetActive(false);
            longPool.Enqueue(l);
        }
    }

    public ShortNote GetShort()
    {
        if (shortPool.Count > 0)
        {
            var n = shortPool.Dequeue();
            n.gameObject.SetActive(true);
            n.transform.position = spawnPos;
            return n;
        }
        return Instantiate(shortPrefab);
    }

    public LongNote GetLong()
    {
        if (longPool.Count > 0)
        {
            var n = longPool.Dequeue();
            n.gameObject.SetActive(true);
            n.transform.position = spawnPos;
            return n;
        }
        return Instantiate(longPrefab);
    }

    // 반환
    public void Return(NoteObject _note)
    {
        _note.gameObject.SetActive(false);

        if (_note is ShortNote s)
            shortPool.Enqueue(s);
        else if (_note is LongNote l)
            longPool.Enqueue(l);
    }

    // 등록 / 제거
    public void Register(NoteObject _note)
    {
        notesByLane[_note.note.lane].Enqueue(_note);
    }

    public void Unregister(NoteObject _note)
    {
        int lane = _note.note.lane;

        if (notesByLane[lane].Count > 0 && notesByLane[lane].Peek() == _note)
        {
            notesByLane[lane].Dequeue();
        }
    }

    public void TryHitLane(int _lane)
    {
        if (notesByLane[_lane].Count == 0) 
            return;

        notesByLane[_lane].Peek().TryHit();
    }

    public void HoldLane(int lane, bool isHolding)
    {
        if (activeLongNotes.TryGetValue(lane, out LongNote ln))
        {
            ln.SetHoldInput(isHolding);
        }
    }

    // 등록
    public void SetActiveLongNote(int lane, LongNote note)
    {
        activeLongNotes[lane] = note;
    }

    // 해제
    public void ClearActiveLongNote(int lane)
    {
        if (activeLongNotes.ContainsKey(lane))
            activeLongNotes.Remove(lane);
    }
}