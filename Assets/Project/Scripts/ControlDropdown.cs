using TMPro;
using UnityEngine;

public class ControlDropdown : MonoBehaviour
{
    public PlayerControlMapping controlMapping;
    private TMP_Dropdown dropdown;
    public string mappingFieldName; 
    private string[] actionNames = new string[]
    {
        "LowPunch",
        "LowKick",
        "HardPunch",
        "HardKick",
        "Block",
        "Throw"
    };

    private string[] displayNames = new string[]
    {
        "J/X",
        "L/B",
        "I/Y",
        "K/A",
        "O/RB",
        "U/LB"
    };

    void Start()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        dropdown.ClearOptions();
        dropdown.AddOptions(new System.Collections.Generic.List<string>(displayNames));

        string currentValue = (string)controlMapping.GetType().GetField(mappingFieldName).GetValue(controlMapping);

        int index = System.Array.IndexOf(actionNames, currentValue);
        dropdown.value = index >= 0 ? index : 0;

        dropdown.onValueChanged.AddListener(OnDropdownChanged);
    }

    void OnDropdownChanged(int index)
    {
        controlMapping.GetType().GetField(mappingFieldName).SetValue(controlMapping, actionNames[index]);

        // playerControl.ReloadMapping();
    }
}

