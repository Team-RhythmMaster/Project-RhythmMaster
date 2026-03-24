using UnityEngine;
using System.Collections.Generic;

public class NoteManager : MonoBehaviour
{
    private static NoteManager instance;
    public static NoteManager Instance {  get { return instance; } }

    // 현재 활성화된 노트 리스트
    public List<NoteObject> notes = new List<NoteObject>();

    // Object Pooling
    private Queue<GameObject> pool = new Queue<GameObject>();
    public Transform notePool;
    public GameObject notePrefab;
    public int poolSize = 100;

    void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        Init();
    }

    private void Init()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(notePrefab, notePool);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    // 노트 등록
    public void Register(NoteObject note)
    {
        notes.Add(note);
    }

    // 노트 제거
    public void Unregister(NoteObject note)
    {
        notes.Remove(note);
    }

    // 특정 레인에서 가장 가까운 노트 찾기
    public NoteObject GetClosestNote(int lane, float currentTime)
    {
        NoteObject closest = null;
        float minDiff = float.MaxValue;

        foreach (var note in notes)
        {
            if (note.lane != lane) 
                continue;

            float diff = Mathf.Abs(note.noteTime - currentTime);

            if (diff < minDiff)
            {
                minDiff = diff;
                closest = note;
            }
        }

        return closest;
    }

    //public GameObject Get()
    //{
    //    if (pool.Count > 0)
    //    {
    //        GameObject obj = pool.Dequeue();
    //        obj.SetActive(true);
    //        return obj;
    //    }

    //    // 부족하면 추가 생성
    //    return Instantiate(notePrefab);
    //}

    //public void Return(GameObject obj)
    //{
    //    obj.SetActive(false);
    //    pool.Enqueue(obj);
    //}
}