using System;
using UnityEngine;
using System.Collections.Generic;
using Utils.GameDefinitions;

namespace Utils.ClassUtility
{
    public static class ClassUtility
    {
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            T component = gameObject.GetComponent<T>();
            if (component == null)
            {
                component = gameObject.AddComponent<T>();
            }
            return component;
        }
    }

    [Serializable]
    public class Note
    {
        public NoteType type; // 노트 타입 (0: Short, 1: Long)
        public float time;    // 노트 시작 시간 (언제 도착할지)
        public float tail;    // 롱노트 끝
        public int line;      // 레인 index
    }

    [Serializable]
    public class Sheet
    {
        // Description
        public string title;
        public string artist;

        // Audio
        public int bpm;
        public int offset;
        public int[] signature;

        // Note
        public List<Note> notes = new List<Note>();


        public AudioClip clip;
        public Sprite img;

        public float BarPerSec { get; private set; }
        public float BeatPerSec { get; private set; }

        public int BarPerMilliSec { get; private set; }
        public int BeatPerMilliSec { get; private set; }

        public void Init()
        {
            BarPerMilliSec = (int)(signature[0] / (bpm / 60f) * 1000);
            BeatPerMilliSec = BarPerMilliSec / 64;

            BarPerSec = BarPerMilliSec * 0.001f;
            BeatPerSec = BarPerMilliSec / 64f;
        }
    }
}