using TMPro;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI highScoreText = null;

    private PlayerStatus playerStatus;


    private void Start()
    {
        playerStatus = PlayerStatus.Instance;

        InitializePlayerStatus();

        /// Set HighScore from status file.
        highScoreText.text = "High Score: " +  playerStatus.GetHighScore().ToString();
    }

    private void InitializePlayerStatus()
    {
        /// Initialize Player Status and create/load saved file.
        playerStatus.Initialize();
        playerStatus.LoadJson();
    }
}
