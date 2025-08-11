using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject HUDPanel;
    public GameObject PausePanel;
    public GameObject PauseOptionPanel;
    public GameObject PauseAudioPanel;
    public GameObject PauseVideoPanel;
    public GameObject PauseControlPanel;
    public GameObject WinPanel;
    public GameObject LosePanel;
    public GameObject VersusFinalPanel;

    private GameManager gameManager;

    public GameObject roundTextObject;

    private MenuManager menu;
    public TMPro.TextMeshProUGUI versusWinnerText;

    
    public float roundTime = 99f;
    private float currentTime;
    public TextMeshProUGUI timerText;

    void Start()
    {
        HUD();
        gameManager = FindFirstObjectByType<GameManager>();

        AudioManager.Instance.PlayOneShot("FightEffect");
        int round = RoundText.Instance != null ? RoundText.Instance.currentRound : 1;
        ShowRoundText(round);
        currentTime = roundTime;
    }

    void Update()
    {
        HandleTimer();
    }

    private void HandleTimer()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            if (currentTime < 0) currentTime = 0;
        }
        else
        {
            gameManager.TimeOutRound();
        }

        if (timerText != null)
        {
            timerText.text = Mathf.CeilToInt(currentTime).ToString();
        }
    }

    public void HUD()
    {
        Time.timeScale = 1f;
        HUDPanel.SetActive(true);
        PausePanel.SetActive(false);
        WinPanel.SetActive(false);
        PauseOptionPanel.SetActive(false);
        PauseAudioPanel.SetActive(false);
        PauseVideoPanel.SetActive(false);
        PauseControlPanel.SetActive(false);
        AudioManager.Instance.Play("Fight1");
    }

    public void pause()
    {
        Time.timeScale = 0f;
        HUDPanel.SetActive(false);
        PausePanel.SetActive(true);
        WinPanel.SetActive(false);
        PauseOptionPanel.SetActive(true);
        PauseAudioPanel.SetActive(false);
        PauseVideoPanel.SetActive(false);
        PauseControlPanel.SetActive(false);
    }

    public void Win()
    {
        Time.timeScale = 0f;
        HUDPanel.SetActive(false);
        WinPanel.SetActive(true);
        PauseOptionPanel.SetActive(false);
        PauseAudioPanel.SetActive(false);
        PauseVideoPanel.SetActive(false);
        PauseControlPanel.SetActive(false);
        AudioManager.Instance.Play("Victory");
    }

    public void WinBotton()
    {
        gameManager.EndMatch();
    }

    public void Lose()
    {
        Time.timeScale = 0f;
        HUDPanel.SetActive(false);
        LosePanel.SetActive(true);
        PauseOptionPanel.SetActive(false);
        PauseAudioPanel.SetActive(false);
        PauseVideoPanel.SetActive(false);
        PauseControlPanel.SetActive(false);
        AudioManager.Instance.Play("Defeat");
    }

    public void LoseBotton()
    {
        gameManager.ResetMatch();
    }

    public void Versus()
    {
        Time.timeScale = 0f;
        HUDPanel.SetActive(false);
        VersusFinalPanel.SetActive(true);
        AudioManager.Instance.Play("VersusPanel");
    }
    public void exitToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void audioPauseOptions()
    {
        PauseOptionPanel.SetActive(false);
        PauseAudioPanel.SetActive(true);
    }
    public void videoPauseOptions()
    {
        PauseOptionPanel.SetActive(false);
        PauseVideoPanel.SetActive(true);
    }

    public void controlPauseOptions()
    {
        PauseOptionPanel.SetActive(false);
        PauseControlPanel.SetActive(true);
    }
    
    public void ShowRoundText(int number)
    {
        roundTextObject.SetActive(true);
        roundTextObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Round " + number;
        Invoke(nameof(HideRoundText), 1f); 
    }

    void HideRoundText()
    {
        roundTextObject.SetActive(false);
    }

}
