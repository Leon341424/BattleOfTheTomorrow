using UnityEngine.UI;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance;
    public GameObject selectedCharacterPlayer1;
    public GameObject selectedCharacterPlayer2;
    public GameObject selectedCharacterEnemy;

    public Sprite hudImagePlayer1;
    public Sprite hudImagePlayer2;

    public GameMode currentMode;

    private void Awake()
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
}

public enum GameMode
{
    Arcade,
    VersusCPU,
    VersusPvP
}