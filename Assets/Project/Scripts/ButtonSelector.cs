using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSelector : MonoBehaviour
{
    public GameObject firstButton;

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
}
