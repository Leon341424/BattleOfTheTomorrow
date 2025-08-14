using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CharacterSpawn : MonoBehaviour
{
    public Transform spawnPoint;
    public Transform spawnPointPlayer2;

    public Image ImagePlayer1;
    public Image ImagePlayer2;


    void Start()
    {
        ImagePlayer1.sprite = CharacterManager.Instance.hudImagePlayer1;
        ImagePlayer2.sprite = CharacterManager.Instance.hudImagePlayer2;

        var cm = CharacterManager.Instance;

        if (CharacterManager.Instance != null && CharacterManager.Instance.selectedCharacterPlayer1 != null)
        {
            if (cm.selectedCharacterPlayer1 != null)
            {
                Instantiate(CharacterManager.Instance.selectedCharacterPlayer1, spawnPoint.position, Quaternion.identity);
            }
            else
            {
                Debug.Log("No hay personaje seleccionado");
            }
        }

        if (cm.currentMode == GameMode.Arcade)
        {
            if (cm.selectedCharacterEnemy != null)
            {
                Instantiate(CharacterManager.Instance.selectedCharacterEnemy, spawnPointPlayer2.position, Quaternion.identity);
            }
            else
            {
                Debug.Log("No hay personaje seleccionado");
            }
        }

        if (cm.currentMode == GameMode.VersusCPU)
        {
            if (cm.selectedCharacterEnemy != null)
            {
                Instantiate(CharacterManager.Instance.selectedCharacterEnemy, spawnPointPlayer2.position, Quaternion.identity);
            }
            else
            {
                Debug.Log("No hay personaje seleccionado");
            }
        }

        if (cm.currentMode == GameMode.VersusPvP)
        {
            if (cm.selectedCharacterPlayer2 != null)
            {
                Instantiate(CharacterManager.Instance.selectedCharacterPlayer2, spawnPointPlayer2.position, Quaternion.identity);
            }
            else
            {
                Debug.Log("No hay segundo personaje");
            }
        }    
    }
}
