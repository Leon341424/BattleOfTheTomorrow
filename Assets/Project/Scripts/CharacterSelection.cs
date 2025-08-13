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
    public GameObject character2P2Prefab;
    public GameObject character3P2Prefab;
    public GameObject character4P2Prefab;
    public GameObject character5P2Prefab;
    public GameObject character6P2Prefab;
    public GameObject character7P2Prefab;
    public GameObject character8P2Prefab;
    public GameObject character1P1PrefabKeyboard;
    public GameObject character2P1PrefabKeyboard;
    public GameObject character3P1PrefabKeyboard;
    public GameObject character4P1PrefabKeyboard;
    public GameObject character5P1PrefabKeyboard;
    public GameObject character6P1PrefabKeyboard;
    public GameObject character7P1PrefabKeyboard;
    public GameObject character8P1PrefabKeyboard;
    public GameObject character1EnemyPrefab;
    public GameObject character2EnemyPrefab;
    public GameObject character3EnemyPrefab;
    public GameObject character4EnemyPrefab;
    public GameObject character5EnemyPrefab;
    public GameObject character6EnemyPrefab;
    public GameObject character7EnemyPrefab;
    public GameObject character8EnemyPrefab;
    public Toggle versusToggle;

    //Jugador 1 ambos controles
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

    //Jugador 2 solo control
    public void SelectCharacter1P2()
    {
        CharacterManager.Instance.selectedCharacterPlayer2 = character1P2Prefab;
    }
    public void SelectCharacter2P2()
    {
        CharacterManager.Instance.selectedCharacterPlayer2 = character2P2Prefab;
    }
    public void SelectCharacter3P2()
    {
        CharacterManager.Instance.selectedCharacterPlayer2 = character3P2Prefab;
    }
    public void SelectCharacter4P2()
    {
        CharacterManager.Instance.selectedCharacterPlayer2 = character4P2Prefab;
    }
    public void SelectCharacter5P2()
    {
        CharacterManager.Instance.selectedCharacterPlayer2 = character5P2Prefab;
    }
    public void SelectCharacter6P2()
    {
        CharacterManager.Instance.selectedCharacterPlayer2 = character6P2Prefab;
    }
    public void SelectCharacter7P2()
    {
        CharacterManager.Instance.selectedCharacterPlayer2 = character7P2Prefab;
    }
    public void SelectCharacter8P2()
    {
        CharacterManager.Instance.selectedCharacterPlayer2 = character8P2Prefab;
    }

    //Jugador 1 solo teclado
    public void SelectCharacter1P1Keyboard()
    {
        CharacterManager.Instance.selectedCharacterPlayer1 = character1P1PrefabKeyboard;
    }
    public void SelectCharacter2P1Keyboard()
    {
        CharacterManager.Instance.selectedCharacterPlayer1 = character2P1PrefabKeyboard;
    }
    public void SelectCharacter3P1Keyboard()
    {
        CharacterManager.Instance.selectedCharacterPlayer1 = character3P1PrefabKeyboard;
    }
    public void SelectCharacter4P1Keyboard()
    {
        CharacterManager.Instance.selectedCharacterPlayer1 = character4P1PrefabKeyboard;
    }
    public void SelectCharacter5P1Keyboard()
    {
        CharacterManager.Instance.selectedCharacterPlayer1 = character5P1PrefabKeyboard;
    }
    public void SelectCharacter6P1Keyboard()
    {
        CharacterManager.Instance.selectedCharacterPlayer1 = character6P1PrefabKeyboard;
    }
    public void SelectCharacter7P1Keyboard()
    {
        CharacterManager.Instance.selectedCharacterPlayer1 = character7P1PrefabKeyboard;
    }
    public void SelectCharacter8P1Keyboard()
    {
        CharacterManager.Instance.selectedCharacterPlayer1 = character8P1PrefabKeyboard;
    }

    //Enemigo IA
    public void SelectCharacter1Enemy()
    {
        CharacterManager.Instance.selectedCharacterEnemy = character1EnemyPrefab;
    }
    public void SelectCharacter2Enemy()
    {
        CharacterManager.Instance.selectedCharacterEnemy = character2EnemyPrefab;
    }
    public void SelectCharacter3Enemy()
    {
        CharacterManager.Instance.selectedCharacterEnemy = character3EnemyPrefab;
    }
    public void SelectCharacter4Enemy()
    {
        CharacterManager.Instance.selectedCharacterEnemy = character4EnemyPrefab;
    }
    public void SelectCharacter5Enemy()
    {
        CharacterManager.Instance.selectedCharacterEnemy = character5EnemyPrefab;
    }
    public void SelectCharacter6Enemy()
    {
        CharacterManager.Instance.selectedCharacterEnemy = character6EnemyPrefab;
    }
    public void SelectCharacter7Enemy()
    {
        CharacterManager.Instance.selectedCharacterEnemy = character7EnemyPrefab;
    }
    public void SelectCharacter8Enemy()
    {
        CharacterManager.Instance.selectedCharacterEnemy = character8EnemyPrefab;
    }

    //Randoms
    //jugador 1
    public void SelectRandomCharacterP1()
    {
        GameObject[] charactersP1 = new GameObject[]
        {
            character1P1Prefab,
            character2P1Prefab,
            character3P1Prefab,
            character4P1Prefab,
            character5P1Prefab,
            character6P1Prefab,
            character7P1Prefab,
            character8P1Prefab
        };

        int randomIndex = Random.Range(0, charactersP1.Length);
        CharacterManager.Instance.selectedCharacterPlayer1 = charactersP1[randomIndex];
    }

    //Jugador 2
    public void SelectRandomCharacterP2()
    {
        GameObject[] charactersP2 = new GameObject[]
        {
            character1P2Prefab,
            character2P2Prefab,
            character3P2Prefab,
            character4P2Prefab,
            character5P2Prefab,
            character6P2Prefab,
            character7P2Prefab,
            character8P2Prefab
        };

        int randomIndex = Random.Range(0, charactersP2.Length);
        CharacterManager.Instance.selectedCharacterPlayer2 = charactersP2[randomIndex];
    }

    //jugador 1 teclado
    public void SelectRandomCharacterP1Keyboard()
    {
        GameObject[] charactersP1Keyboard = new GameObject[]
        {
            character1P1PrefabKeyboard,
            character2P1PrefabKeyboard,
            character3P1PrefabKeyboard,
            character4P1PrefabKeyboard,
            character5P1PrefabKeyboard,
            character6P1PrefabKeyboard,
            character7P1PrefabKeyboard,
            character8P1PrefabKeyboard
        };

        int randomIndex = Random.Range(0, charactersP1Keyboard.Length);
        CharacterManager.Instance.selectedCharacterPlayer1 = charactersP1Keyboard[randomIndex];
    }

    //Enemigos
    public void SelectRandomCharacterEnemy()
    {
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

        int randomIndex = Random.Range(0, enemies.Length);
        CharacterManager.Instance.selectedCharacterEnemy = enemies[randomIndex];
    }

    //Seleccionar modo de juego
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
