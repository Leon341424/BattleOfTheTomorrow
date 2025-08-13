using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterSpawn : MonoBehaviour
{
    public Transform spawnPoint;
    public Transform spawnPointPlayer2;

    void Start()
    {
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
