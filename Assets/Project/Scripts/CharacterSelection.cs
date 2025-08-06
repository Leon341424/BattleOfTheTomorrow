using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    public GameObject character1P1Prefab;
    public GameObject character1P2Prefab;
    public Toggle versusToggle;
    public void SelectCharacter1P1()
    {
        CharacterManager.Instance.selectedCharacterPlayer1 = character1P1Prefab;
    }

    public void SelectCharacter1P2()
    {
        CharacterManager.Instance.selectedCharacterPlayer2 = character1P2Prefab;
    }

    public void SelectArcadeMode()
    {
        CharacterManager.Instance.currentMode = GameMode.Arcade;
        Debug.Log("MODO Arcade");
    }

    public void SetVersusMode()
    {
        if (versusToggle.isOn)
        {
            CharacterManager.Instance.currentMode = GameMode.VersusPvP;
            Debug.Log("MODO PVP");
        }
        else
        {
            CharacterManager.Instance.currentMode = GameMode.VersusCPU;
            Debug.Log("MODO PVE");
        }
    }

    public void SelectVersusCPUMode()
    {
        CharacterManager.Instance.currentMode = GameMode.VersusCPU;
        Debug.Log("MODO PVE");
    }
}
