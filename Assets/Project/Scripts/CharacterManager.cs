using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance;
    public GameObject selectedCharacter;

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
