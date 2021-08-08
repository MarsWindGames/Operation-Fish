using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    public static ScoreManager instance;

    public int playerScore;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        playerScore = PlayerPrefs.GetInt("playerScore", 0);

    }

    public void IncreaseScore(int amount)
    {
        playerScore += amount;
        SaveScore();
    }

    public void DecreaseScore(int amount)
    {
        playerScore -= amount;
        SaveScore();
    }

    void SaveScore()
    {
        PlayerPrefs.SetInt("playerScore", playerScore);
    }

}
