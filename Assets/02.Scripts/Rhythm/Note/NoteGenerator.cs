using UnityEngine;
using Utils.ClassUtility;
using System.Collections.Generic;

// 노트 생성 및 타이밍 제어
public class NoteGenerator : MonoBehaviour
{
    //// 시간순으로 정렬된 노트 스케쥴 리스트
    //public List<NoteData> notes = new List<NoteData>();

    //public float speed { get { return noteSpeed; } }
    //private float noteSpeed = 3.5f;

    //private float spawnAheadTime; // 노트가 판정선까지 이동하는 데 걸리는 시간
    //private int spawnIndex = 0;   // 다음에 생성할 노트 인덱스
    //private float rightEdge;      // 화면 오른쪽 끝 위치
    //private float spawnX;         // 노트 시작 위치

    //private void Update()
    //{
    //    float currentTime = AudioManager.Instance.songTime;

    //    while (spawnIndex < notes.Count)
    //    {
    //        // 도착 시간 - 이동 시간 = 생성 시간
    //        if (notes[spawnIndex].time <= currentTime + spawnAheadTime)
    //        {
    //            Spawn(notes[spawnIndex]);
    //            spawnIndex++;
    //        }
    //        else
    //            break;
    //    }
    //}

    //public void Init()
    //{
    //    rightEdge = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
    //    spawnX = rightEdge + 2.0f;

    //    // 거리 / 속도 = 시간 (+1.0f → 여유 시간/렉 방지/프레임 드랍 방지)
    //    spawnAheadTime = ((spawnX - NoteManager.hitLineX) / speed) + 1.0f;
    //    double currentTime = AudioManager.Instance.songTime;

    //    // 초기 생성 → 게임 시작 시 이미 보여야 할 노트들 미리 생성
    //    while (spawnIndex < notes.Count)
    //    {
    //        if (notes[spawnIndex].time <= currentTime + spawnAheadTime)
    //        {
    //            Spawn(notes[spawnIndex]);
    //            spawnIndex++;
    //        }
    //        else
    //            break;
    //    }
    //}

    //// 노트 리스트에 추가
    //public void AddNote(NoteData _note)
    //{
    //    notes.Add(_note);
    //    notes.Sort((a, b) => a.time.CompareTo(b.time));
    //}

    //// 노트 화면에 생성
    //private void Spawn(NoteData _data)
    //{
    //    float y = NoteManager.Instance.GetLaneY(_data.lane);
    //    NoteObject note = NoteManager.Instance.GetNote(_data);

    //    note.transform.position = new Vector3(spawnX, y, 0);
    //    note.Init(_data, speed);
    //}

    public List<NoteData> notes = new List<NoteData>();

    public float speed => noteSpeed;
    private float noteSpeed = 3.5f;

    private float spawnAheadTime;
    private float spawnX;
    private float rightEdge;

    // lane별 노트 리스트
    private Dictionary<int, List<NoteData>> laneNotes = new();
    // lane별 활성화 인덱스
    private Dictionary<int, int> laneIndex = new();
    // lane별 미리 생성된 노트
    private Dictionary<int, List<NoteObject>> laneObjects = new();

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        ActivateNotes();
    }

    public void Init()
    {
        rightEdge = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
        spawnX = rightEdge + 2.0f;

        spawnAheadTime = ((spawnX - NoteManager.hitLineX) / speed) + 1.0f;

        // lane별 분리
        SplitByLane();

        // 미리 생성
        PreloadAllNotes();
    }

    private void SplitByLane()
    {
        laneNotes.Clear();
        laneIndex.Clear();
        laneObjects.Clear();

        foreach (var note in notes)
        {
            if (!laneNotes.ContainsKey(note.lane))
            {
                laneNotes[note.lane] = new List<NoteData>();
                laneIndex[note.lane] = 0;
                laneObjects[note.lane] = new List<NoteObject>();
            }

            laneNotes[note.lane].Add(note);
        }

        // lane별 정렬
        foreach (var kv in laneNotes)
        {
            kv.Value.Sort((a, b) => a.time.CompareTo(b.time));
        }
    }

    private void PreloadAllNotes()
    {
        foreach (var kv in laneNotes)
        {
            int lane = kv.Key;
            var list = kv.Value;

            float y = NoteManager.Instance.GetLaneY(lane);

            foreach (var data in list)
            {
                NoteObject note = NoteManager.Instance.GetNote(data);

                note.transform.position = new Vector3(spawnX, y, 0);

                note.Init(data, speed);

                note.gameObject.SetActive(false);

                laneObjects[lane].Add(note);
            }
        }
    }

    private void ActivateNotes()
    {
        float currentTime = AudioManager.Instance.songTime;

        foreach (var lane in laneNotes.Keys)
        {
            var notesList = laneNotes[lane];
            var objList = laneObjects[lane];

            int index = laneIndex[lane];

            while (index < notesList.Count)
            {
                if (notesList[index].time <= currentTime + spawnAheadTime)
                {
                    objList[index].gameObject.SetActive(true);
                    index++;
                }
                else break;
            }

            laneIndex[lane] = index;
        }
    }
}