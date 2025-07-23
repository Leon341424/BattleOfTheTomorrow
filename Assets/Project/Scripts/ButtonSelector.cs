using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSelector : MonoBehaviour
{
    public GameObject firstButton;

    void Start()
    {
        EventSystem.current.SetSelectedGameObject(null); // Limpia selección previa
        EventSystem.current.SetSelectedGameObject(firstButton); // Selecciona el primero
    }
}
