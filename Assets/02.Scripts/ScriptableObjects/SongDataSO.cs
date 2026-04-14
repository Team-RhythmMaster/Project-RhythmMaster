using UnityEngine;
using Utils.EnumType;
using Utils.ClassUtility;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SongData", menuName = "Rhythm/SongData")]
public class SongDataSO : ScriptableObject
{
    // Description
    public int id;
    public int bpm;
    public string title;
    public string artist;
    public Difficulty difficulty;

    public List<NoteData> notes = new List<NoteData>();
    public AudioClip audioClip;
    public Sprite songSprite;
}