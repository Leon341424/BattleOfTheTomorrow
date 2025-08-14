using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum GameMode { Arcade, Versus }
    public GameMode currentMode;

    private PlayerHealth player1Health;
    private EnemyHealth player2Health;

    private int player1Wins = 0;
    private int player2Wins = 0;

    private int currentStageIndex = 0;
    public List<string> arcadeScenes;
    public string versusScene;

    private UIManager WinOrLose;
    
    public GameObject character1EnemyPrefab;
    public GameObject character2EnemyPrefab;
    public GameObject character3EnemyPrefab;
    public GameObject character4EnemyPrefab;
    public GameObject character5EnemyPrefab;
    public GameObject character6EnemyPrefab;
    public GameObject character7EnemyPrefab;
    public GameObject character8EnemyPrefab;

    public Sprite ImageCharacter1;
    public Sprite ImageCharacter2;
    public Sprite ImageCharacter3;
    public Sprite ImageCharacter4;
    public Sprite ImageCharacter5;
    public Sprite ImageCharacter6;
    public Sprite ImageCharacter7;
    public Sprite ImageCharacter8;

    public GameObject finalBossPrefab;
    public Sprite finalBossImage;


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

    public void TimeOutRound()
    {

        GameObject player1Obj = GameObject.FindWithTag("Player");
        player1Health = player1Obj.GetComponent<PlayerHealth>();

        GameObject player2Obj = GameObject.FindWithTag("Enemy");
        player2Health = player2Obj.GetComponent<EnemyHealth>();

        if (player1Health != null && player2Health != null)
        {
            if (player1Health.GetCurrentHealth() > player2Health.GetCurrentHealth())
            {
                PlayerWonRound(1);
            }
            else if (player2Health.GetCurrentHealth() > player1Health.GetCurrentHealth())
            {
                PlayerWonRound(2);
            }
            else
            {
                Debug.Log("Empate por tiempo");
                PlayerWonRound(1);
            }
        }
        else
        {
            Debug.LogWarning("Faltan referencias a PlayerHealth en GameManager.");
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
            if (currentMode == GameMode.Arcade)
            {
                WinOrLose.Win();
                SelectRandomCharacterEnemy();
            }
            if (currentMode == GameMode.Versus)
            {
                WinOrLose.Versus();
                WinOrLose.versusWinnerText.text = "Player 1 Wins!";
            }
            RoundText.Instance.ResetRounds();
        }
        else if (player2Wins == 2)
        {
            if (currentMode == GameMode.Arcade) WinOrLose.Lose();
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

        /*if (currentMode == GameMode.Versus)
        {
            SceneManager.LoadScene("MainMenu");
        }*/
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
        SelectRandomCharacterEnemy();
        SceneManager.LoadScene(arcadeScenes[0]);
    }

    public void StartVersusMode()
    {
        currentMode = GameMode.Versus;
        ResetRoundWins();
        /*if (arcadeScenes.Count > 0)
        {
            int randomIndex = Random.Range(0, arcadeScenes.Count);
            SceneManager.LoadScene(arcadeScenes[randomIndex]);
        }*/
        SceneManager.LoadScene(versusScene);
    }
    
    public void SelectRandomCharacterEnemy()
    {
        if (currentStageIndex == arcadeScenes.Count - 1)
        {
            CharacterManager.Instance.selectedCharacterEnemy = finalBossPrefab;
            CharacterManager.Instance.hudImagePlayer2 = finalBossImage;
            return;
        }
        GameObject[] enemies = new GameObject[]
        {
            character1EnemyPrefab,
            character2EnemyPrefab,
            character3EnemyPrefab,
            character4EnemyPrefab,
            character5EnemyPrefab,
            character6EnemyPrefab,
            character7EnemyPrefab,
            character8EnemyPrefab
        };
        Sprite[] imagesP2 = new Sprite[]
        {
            ImageCharacter1,
            ImageCharacter2,
            ImageCharacter3,
            ImageCharacter4,
            ImageCharacter5,
            ImageCharacter6,
            ImageCharacter7,
            ImageCharacter8
        };

        int randomIndex = Random.Range(0, enemies.Length);
        CharacterManager.Instance.selectedCharacterEnemy = enemies[randomIndex];
        CharacterManager.Instance.hudImagePlayer2 = imagesP2[randomIndex];
    }
}
