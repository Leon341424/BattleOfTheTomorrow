using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterSpawn : MonoBehaviour
{
    public Transform spawnPoint;
    public Transform spawnPointPlayer2;

    void Start()
    {
        var cm = CharacterManager.Instance;

        if (CharacterManager.Instance != null && CharacterManager.Instance.selectedCharacterPlayer1 != null
        && cm.currentMode == GameMode.Arcade)
        {
            Instantiate(CharacterManager.Instance.selectedCharacterPlayer1, spawnPoint.position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("No hay personaje seleccionado");
        }

        if (cm.currentMode == GameMode.VersusCPU)
        {
            // Instanciar personaje de IA
            /*var ai = Instantiate(aiOpponentPrefab, spawnPointPlayer2.position, Quaternion.identity);
            player1.GetComponent<PlayerControl>()?.SetOpponent(ai.transform);*/
        }

        if (cm.currentMode == GameMode.VersusPvP)
        {
            var player1 = PlayerInput.Instantiate(
            cm.selectedCharacterPlayer1,
            controlScheme: "Keyboard&Mouse",
            pairWithDevice: Keyboard.current,
            playerIndex: 0
            );
            player1.transform.position = spawnPoint.position;
            player1.transform.rotation = spawnPoint.rotation;

            if (cm.selectedCharacterPlayer2 != null)
            {
                var player2 = PlayerInput.Instantiate(
                cm.selectedCharacterPlayer2,
                controlScheme: "Gamepad",
                pairWithDevice: Gamepad.current,
                playerIndex: 1
                );
                player2.transform.position = spawnPointPlayer2.position;
                player2.transform.rotation = spawnPointPlayer2.rotation;
            }
            else
            {
                Debug.Log("No hay segundo personaje o control conectado.");
            }
        }    
    }
}
