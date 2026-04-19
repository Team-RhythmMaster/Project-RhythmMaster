using UnityEngine;
using UnityEngine.UI;
using Utils.EnumType;
using Utils.ClassUtility;

public class ResultUI : MonoBehaviour
{
    private GameObject gameresultPhanel;

    public Sprite[] rankSprites;
    private Image rankImage;

    private Text scoreText;
    private Text prefectText;
    private Text greatText;
    private Text goodText;
    private Text badText;
    private Text missText;
    private Text maxComboText;

    private Button restartButton;
    private Button mainButton;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        gameresultPhanel = transform.Find("Background").gameObject;

        Transform root = transform.Find("Background/ResultPhanel");
        rankImage = root.transform.Find("Rank/RankBG/RankImage").GetComponent<Image>();
        scoreText = root.transform.Find("Rank/ClearCoint").GetComponent<Text>();

        prefectText = root.transform.Find("Score/Perfect").GetComponentInChildren<Text>(true);
        greatText = root.transform.Find("Score/Great/GreatText").GetComponent<Text>();
        goodText = root.transform.Find("Score/Good/GoodText").GetComponent<Text>();
        badText = root.transform.Find("Score/Bad/BadText").GetComponent<Text>();
        missText = root.transform.Find("Score/Miss/MissText").GetComponent<Text>();
        maxComboText = root.transform.Find("Combo/MaxComboText").GetComponent<Text>();

        restartButton = gameresultPhanel.transform.Find("OptionPhanel/RestartButton").GetComponent<Button>();
        mainButton = gameresultPhanel.transform.Find("OptionPhanel/MainButton").GetComponent<Button>();
    }

    public void ShowResultUI(SongRecord _songRecord)
    {
        gameresultPhanel.SetActive(true);
        SetRankImage(_songRecord.rank);
        scoreText.text = _songRecord.maxScore.ToString();
        prefectText.text = ScoreManager.Instance.scoreData.perfect.ToString();
        greatText.text = ScoreManager.Instance.scoreData.great.ToString();
        goodText.text = ScoreManager.Instance.scoreData.good.ToString();
        badText.text = ScoreManager.Instance.scoreData.bad.ToString();
        missText.text = ScoreManager.Instance.scoreData.miss.ToString();
        maxComboText.text = _songRecord.maxCombo.ToString();
    }

    private void SetRankImage(RankType _rank)
    {
        switch(_rank)
        {
            case RankType.S:
                rankImage.sprite = rankSprites[0];
                break;
            case RankType.A:
                rankImage.sprite = rankSprites[1];
                break;
            case RankType.B:
                rankImage.sprite = rankSprites[2];
                break;
            case RankType.C:
                rankImage.sprite = rankSprites[3];
                break;
            case RankType.D:
                rankImage.sprite = rankSprites[4];
                break;
            case RankType.F:
                rankImage.sprite = rankSprites[5];
                break;
        }
    }
}
