using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject HUDPanel;
    public GameObject PausePanel;
    public GameObject PauseOptionPanel;
    public GameObject PauseAudioPanel;
    public GameObject PauseVideoPanel;
    public GameObject PauseControlPanel;

    void Start()
    {
        HUD();
    }

    /*void Update()
    {
        
    }*/

    public void HUD()
    {
        Time.timeScale = 1f;
        HUDPanel.SetActive(true);
        PausePanel.SetActive(false);
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
        PauseOptionPanel.SetActive(true);
        PauseAudioPanel.SetActive(false);
        PauseVideoPanel.SetActive(false);
        PauseControlPanel.SetActive(false);
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

}
