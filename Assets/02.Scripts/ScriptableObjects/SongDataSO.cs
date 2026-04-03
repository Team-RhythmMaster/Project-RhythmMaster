using UnityEngine;
using Utils.EnumType;
using Utils.ClassUtility;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SongData", menuName = "Rhythm/SongData")]
public class SongDataSO : ScriptableObject
{
    // Description
    public string title;
    public string artist;
    public Difficulty difficulty;

    // Audio
    public int bpm;
    public float offset;

    public List<NoteData> notes = new List<NoteData>();
    public AudioClip audioClip;
    public Sprite songSprite;
}