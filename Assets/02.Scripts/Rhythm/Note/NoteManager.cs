using UnityEngine;
using System.Collections.Generic;

public class NoteManager : MonoBehaviour
{
    private static NoteManager instance;
    public static NoteManager Instance { get { return instance; } }

    // Lane별 큐 (판정용)
    private Dictionary<int, Queue<NoteObject>> notesByLane = new Dictionary<int, Queue<NoteObject>>();

    // 풀링 (타입별 분리)
    private Queue<ShortNote> shortPool = new Queue<ShortNote>();
    private Queue<LongNote> longPool = new Queue<LongNote>();

    public ShortNote shortPrefab;
    public LongNote longPrefab;

    public Transform poolParent;
    public int poolSize = 100;

    private Vector3 spawnPos = new Vector3(10.0f, 0.0f, 0.0f);

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
        for (int i = 0; i < 4; i++)
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
    public void Return(NoteObject note)
    {
        note.gameObject.SetActive(false);

        if (note is ShortNote s)
            shortPool.Enqueue(s);
        else if (note is LongNote l)
            longPool.Enqueue(l);
    }

    // 등록 / 제거
    public void Register(NoteObject note)
    {
        notesByLane[note.note.lane].Enqueue(note);
    }

    public void Unregister(NoteObject note)
    {
        int lane = note.note.lane;

        if (notesByLane[lane].Count > 0 && notesByLane[lane].Peek() == note)
        {
            notesByLane[lane].Dequeue();
        }
    }

    public void TryHitLane(int lane)
    {
        if (notesByLane[lane].Count == 0) 
            return;

        notesByLane[lane].Peek().TryHit();
    }

    public void HoldLane(int lane, bool holding)
    {
        if (notesByLane[lane].Count == 0) 
            return;

        if (notesByLane[lane].Peek() is LongNote ln)
        {
            ln.SetHoldInput(holding);
        }
    }
}