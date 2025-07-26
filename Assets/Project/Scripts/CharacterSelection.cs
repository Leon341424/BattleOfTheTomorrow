using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    public GameObject characterPrefab; // Prefab que instancias desde el botón

    public void SelectCharacter()
    {
        CharacterManager.Instance.selectedCharacter = characterPrefab;
    }
}
