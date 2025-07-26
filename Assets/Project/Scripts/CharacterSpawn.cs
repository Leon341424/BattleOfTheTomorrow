using UnityEngine;

public class CharacterSpawn : MonoBehaviour
{
    public Transform spawnPoint;

    void Start()
    {
        if (CharacterManager.Instance != null && CharacterManager.Instance.selectedCharacter != null)
        {
            Instantiate(CharacterManager.Instance.selectedCharacter, spawnPoint.position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("No hay personaje seleccionado");
        }
    }
}
