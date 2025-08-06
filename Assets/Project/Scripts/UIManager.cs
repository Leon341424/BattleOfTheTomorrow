using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

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

    void Start()
    {
        HUD();
        gameManager = FindFirstObjectByType<GameManager>();

        int round = RoundText.Instance != null ? RoundText.Instance.currentRound : 1;
        ShowRoundText(round);
    }

    /*void Update()
    {
        
    }*/

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
