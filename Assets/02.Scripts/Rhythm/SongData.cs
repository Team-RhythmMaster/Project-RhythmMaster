using UnityEngine;
using Utils.ClassUtility;
using Utils.GameDefinitions;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SongData", menuName = "Rhythm/SongData")]
public class SongData : ScriptableObject
{
    public List<Note> easyNotes;
    public List<Note> normalNotes;
    public List<Note> hardNotes;

    public List<Note> GetNotes(Difficulty diff)
    {
        switch (diff)
        {
            case Difficulty.Easy: 
                return easyNotes;
            case Difficulty.Normal: 
                return normalNotes;
            case Difficulty.Hard: 
                return hardNotes;
        }

        return normalNotes;
    }
}