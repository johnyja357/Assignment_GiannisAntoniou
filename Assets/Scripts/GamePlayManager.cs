using TMPro;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    [SerializeField] private GameObject GameOverPopup = null;
    [SerializeField] private TextMeshProUGUI currentScoreText = null;
    [SerializeField] private TextMeshProUGUI currentStrikeText = null;
    [SerializeField] private TextMeshProUGUI endGameScoreText = null;
    [SerializeField] private TextMeshProUGUI gameConditionText = null; 


    public void SetCurrentScoreText(string currentScore)
    {
        currentScoreText.text = currentScore;
    }

    public void SetStrikeText(string strike)
    {
        currentStrikeText.text = "Strike:" + strike;
    }

    public void GameOver(int currentScore)
    {
        EnablePopup(GlobalVariables.TEXT_GAME_OVER, currentScore.ToString());
    }

    public void GameWin(int currentScore)
    {
        EnablePopup(GlobalVariables.TEXT_GAME_WIN, currentScore.ToString());
    }

    private void EnablePopup(string gameCondition, string currentScore)
    {
        gameConditionText.text = gameCondition;
        endGameScoreText.text = currentScore;
        GameOverPopup.SetActive(true);
    }
}
