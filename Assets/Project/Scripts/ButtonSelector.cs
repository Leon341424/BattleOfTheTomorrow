using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSelector : MonoBehaviour
{
    public GameObject firstButton;
    private bool usingMouse = false;

    void OnEnable()
    {
        StartCoroutine(SelectFirstActiveButton());
    }

    private System.Collections.IEnumerator SelectFirstActiveButton()
    {
        yield return null;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstButton);
    }

    void Update()
    {
        // Detecta si el jugador mueve el mouse
        if (Input.mousePresent && (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0))
        {
            if (!usingMouse)
            {
                usingMouse = true;
                EventSystem.current.SetSelectedGameObject(null); 
            }
        }

        else if (Input.anyKeyDown && !Input.GetMouseButtonDown(0))
        {
            usingMouse = false;
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                EventSystem.current.SetSelectedGameObject(firstButton);
            }
        }
    }
    
}
