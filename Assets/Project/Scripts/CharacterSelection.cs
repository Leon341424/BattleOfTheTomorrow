using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    public GameObject character1P1Prefab;
    public GameObject character2P1Prefab;
    public GameObject character3P1Prefab;
    public GameObject character4P1Prefab;
    public GameObject character5P1Prefab;
    public GameObject character6P1Prefab;
    public GameObject character7P1Prefab;
    public GameObject character8P1Prefab;
    public GameObject character1P2Prefab;
    public GameObject character1P1PrefabKeyboard;
    public Toggle versusToggle;

    public void SelectCharacter1P1Keyboard()
    {
        CharacterManager.Instance.selectedCharacterPlayer1 = character1P1PrefabKeyboard;
    }
    public void SelectCharacter1P1()
    {
        CharacterManager.Instance.selectedCharacterPlayer1 = character1P1Prefab;
    }

    public void SelectCharacter2P1()
    {
        CharacterManager.Instance.selectedCharacterPlayer1 = character2P1Prefab;
    }

    public void SelectCharacter3P1()
    {
        CharacterManager.Instance.selectedCharacterPlayer1 = character3P1Prefab;
    }

    public void SelectCharacter4P1()
    {
        CharacterManager.Instance.selectedCharacterPlayer1 = character4P1Prefab;
    }

    public void SelectCharacter5P1()
    {
        CharacterManager.Instance.selectedCharacterPlayer1 = character5P1Prefab;
    }

    public void SelectCharacter6P1()
    {
        CharacterManager.Instance.selectedCharacterPlayer1 = character6P1Prefab;
    }

    public void SelectCharacter7P1()
    {
        CharacterManager.Instance.selectedCharacterPlayer1 = character7P1Prefab;
    }

    public void SelectCharacter8P1()
    {
        CharacterManager.Instance.selectedCharacterPlayer1 = character8P1Prefab;
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
