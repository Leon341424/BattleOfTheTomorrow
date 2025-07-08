using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject optionsPanel;
    public GameObject videoOptionsPanel;
    public GameObject audioOptionsPanel;
    public GameObject controlOptionsPanel;
    public GameObject creditsPanel;
    //private int currentIndex = 0;

    //private Dictionary<string, GameObject> panelDict;
    void Start()
    {
        MainMenu();
    }

    public void exit()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        menuPanel.SetActive(true);
        optionsPanel.SetActive(false);
        videoOptionsPanel.SetActive(false);
        audioOptionsPanel.SetActive(false);
        controlOptionsPanel.SetActive(false);
        creditsPanel.SetActive(false);
    }

    public void Arcade()
    {
        SceneManager.LoadScene("combat");
    }

    public void options()
    {
        menuPanel.SetActive(false);
        optionsPanel.SetActive(true);
        videoOptionsPanel.SetActive(false);
        audioOptionsPanel.SetActive(false);
        controlOptionsPanel.SetActive(false);
    }

    public void videoOptions()
    {
        optionsPanel.SetActive(false);
        videoOptionsPanel.SetActive(true);
    }

    public void audioOptions()
    {
        optionsPanel.SetActive(false);
        audioOptionsPanel.SetActive(true);
    }

    public void controlOptions()
    {
        optionsPanel.SetActive(false);
        controlOptionsPanel.SetActive(true);
    }

    public void credits()
    {
        menuPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }
}
