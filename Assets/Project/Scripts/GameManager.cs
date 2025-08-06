using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum GameMode { Arcade, Versus }
    public GameMode currentMode;

    private int player1Wins = 0;
    private int player2Wins = 0;

    private int currentStageIndex = 0;
    public List<string> arcadeScenes;
    public string versusScene;

    private UIManager WinOrLose;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ResetRoundWins()
    {
        player1Wins = 0;
        player2Wins = 0;
    }

    public void PlayerWonRound(int player)
    {
        if (player == 1) player1Wins++;
        if (player == 2) player2Wins++;

        WinOrLose = FindFirstObjectByType<UIManager>();

        if (player1Wins == 2)
        {
            if(currentMode == GameMode.Arcade) WinOrLose.Win();
            if (currentMode == GameMode.Versus)
            {
                WinOrLose.Versus(); 
                WinOrLose.versusWinnerText.text = "Player 1 Wins!";  
            } 
            RoundText.Instance.ResetRounds();
        }
        else if (player2Wins == 2)
        {
            if(currentMode == GameMode.Arcade) WinOrLose.Lose();
            if (currentMode == GameMode.Versus)
            {
                WinOrLose.Versus();
                WinOrLose.versusWinnerText.text = "Player 2 Wins!";           
            }  
            RoundText.Instance.ResetRounds();
        }
        else
        {
            RoundText.Instance.AdvanceRound();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void EndMatch()
    {
        if (currentMode == GameMode.Arcade)
        {
            currentStageIndex++;
            if (currentStageIndex < arcadeScenes.Count)
            {
                ResetRoundWins();
                SceneManager.LoadScene(arcadeScenes[currentStageIndex]);
            }
            else
            {
                SceneManager.LoadScene("VictoryScene");
            }
        }

        if (currentMode == GameMode.Versus)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void ResetMatch()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        ResetRoundWins();
    }

    public void StartArcadeMode()
    {
        currentMode = GameMode.Arcade;
        currentStageIndex = 0;
        ResetRoundWins();
        SceneManager.LoadScene(arcadeScenes[0]);
    }

    public void StartVersusMode()
    {
        currentMode = GameMode.Versus;
        ResetRoundWins();
        SceneManager.LoadScene(versusScene);
    }
}
