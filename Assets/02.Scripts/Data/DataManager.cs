using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private static DataManager instance;
    public static DataManager Instance { get { return instance; } }

    public string songDataFileName = "SongData.json";

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
        }
    }

    // 데이터 호출
    public T LoadJson<T>(string _fileName) where T : new()
    {
        string path = Path.Combine(Application.persistentDataPath, "/", _fileName);

        if (File.Exists(path))
        {
            // 이미 저장된 파일이 있다면 불러오기
            string json = File.ReadAllText(path);

            // 읽어온 base-64 인코딩된 문자열을 바이트배열로 변환
            byte[] bytes = System.Convert.FromBase64String(json);
            // 8비트 부호없은 정수를 json 문자열로 변환
            string decodedJson = System.Text.Encoding.UTF8.GetString(bytes);

            return JsonUtility.FromJson<T>(decodedJson);
        }
        else
        {
            // 저장된 파일이 없다면 Resources 폴더에서 기본 데이터 불러오기
            TextAsset baseJson = Resources.Load<TextAsset>("JSON/" + _fileName.Replace(".json", ""));
            if (baseJson != null)
                return JsonUtility.FromJson<T>(baseJson.text);
        }

        // 빈 데이터 반환
        return new T();
    }

    // 데이터 저장
    public void SaveJson<T>(T _data, string _fileName)
    {
        string path = Path.Combine(Application.persistentDataPath, "/", _fileName);
        string json = JsonUtility.ToJson(_data, true);

        // 데이터 암호화
        // json 문자열을 8비트 부호없는 정수로 변환
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(json);
        // 변환된 바이트배열을 base-64 인코딩된 문자열로 변환
        string encodedJson = System.Convert.ToBase64String(bytes);

        File.WriteAllText(path, encodedJson);
    }
}
