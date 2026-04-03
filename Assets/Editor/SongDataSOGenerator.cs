using System.IO;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEngine;
using Utils.ClassUtility;
using Utils.EnumType;

public class SongDataSOGenerator
{
    private const string dataPath = "Assets/Resources/Data/JSON/SongData.json";

    [MenuItem("Tools/Import Song JSON")]
    public static void Generate()
    {
        string data = File.ReadAllText(dataPath);
        SongDataList dataList = JsonUtility.FromJson<SongDataList>(data);

        foreach(var _data in dataList.Songs)
        {
            SongDataSO so = ScriptableObject.CreateInstance<SongDataSO>();

            so.title = _data.title;
            so.artist = _data.artist;
            so.difficulty = (Difficulty)_data.difficulty;
            so.bpm = _data.bpm;
            so.offset = _data.offset;

            so.audioClip = FindAsset<AudioClip>(_data.audioClip, "06.Sounds");
            so.songSprite = FindAsset<Sprite>(_data.songSprite, "04.Images");

            foreach (var n in _data.notes)
            {
                NoteData note = new NoteData
                {
                    lane = n.lane,
                    time = n.time,
                    endTime = n.endTime
                };

                so.notes.Add(note);
            }

            string savePath = $"Assets/08.ScriptableObjects/Songs/{so.title}.asset";
            // ScriptableObject를 실제 에셋 파일로 생성
            AssetDatabase.CreateAsset(so, savePath);
        }

        // 생성된 모든 에셋을 디스크에 저장
        AssetDatabase.SaveAssets();
        // 에디터 에셋 목록 갱신
        AssetDatabase.Refresh();
        Debug.Log("SongData Created");
    }

    static T FindAsset<T>(string _fileName, string _folderFilter) where T : Object
    {
        string[] guids = AssetDatabase.FindAssets($"{_fileName} t:{typeof(T).Name}");

        foreach (var guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);

            // 특정 폴더만 허용
            if (path.Contains(_folderFilter))
            {
                return AssetDatabase.LoadAssetAtPath<T>(path);
            }
        }

        Debug.LogError($"{typeof(T).Name} not found: {_fileName}");
        return null;
    }
}
