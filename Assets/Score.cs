using UnityEngine.UI;
using UnityEngine;

public class Score : MonoBehaviour
{
    public KeyCode DebugScoreIncrease;

    private int ScoreNum;
    private Text ScoreText;

    void Start()
    {
        ScoreText = this.GetComponentInParent<Text>();
        ScoreNum = int.Parse(this.GetComponentInParent<Text>().text);
    }

    public void UpdateScore(int AddedScore)
    {
        Debug.Log("Adding " + AddedScore);
        ScoreNum += AddedScore;
        ScoreText.text = ScoreNum.ToString();
        Debug.Log("Current score:" + ScoreNum);
    }
}
