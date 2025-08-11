using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ChangeControls : MonoBehaviour
{
    public InputSystem_Actions control;
    public TMP_Dropdown[] dropdowns; // Asignar 6 dropdowns en inspector, en orden

    private string[] actionNames = new string[]
    {
        "LowPunch",
        "LowKick",
        "HardPunch",
        "HardKick",
        "Block",
        "Throw"
    };

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (control == null)
        {
            control = new InputSystem_Actions();
            control.Enable();
        }

        foreach (var dropdown in dropdowns)
        {
            dropdown.ClearOptions();
            dropdown.AddOptions(new System.Collections.Generic.List<string>(actionNames));
        }

        LoadBindings();
        SetDropdownsToCurrentBindings();
    }

    public void ApplyAllChanges()
    {
        var playerMap = control.asset.FindActionMap("Player");
        if (playerMap == null)
        {
            Debug.LogError("No se encontró el Action Map 'Player'");
            return;
        }

        var originalBindings = new System.Collections.Generic.Dictionary<string, string[]>();

        foreach (var actionName in actionNames)
        {
            var action = playerMap.FindAction(actionName);
            if (action == null)
            {
                Debug.LogError($"No se encontró la acción {actionName} en Player");
                continue;
            }

            string[] bindings = new string[action.bindings.Count];
            for (int i = 0; i < action.bindings.Count; i++)
            {
                bindings[i] = action.bindings[i].effectivePath;
            }
            originalBindings[actionName] = bindings;
        }

        for (int i = 0; i < dropdowns.Length; i++)
        {
            string originalActionName = actionNames[i];
            string selectedActionName = actionNames[dropdowns[i].value];

            var targetAction = playerMap.FindAction(originalActionName);

            if (targetAction == null || !originalBindings.ContainsKey(selectedActionName))
            {
                Debug.LogError("Error al encontrar acción para reasignar bindings.");
                continue;
            }

            string[] sourceBindings = originalBindings[selectedActionName];

            for (int j = 0; j < sourceBindings.Length; j++)
            {
                if (j < targetAction.bindings.Count)
                {
                    targetAction.ApplyBindingOverride(j, sourceBindings[j]);
                }
            }
        }

        // Forzar refresco
        playerMap.Disable();
        playerMap.Enable();

        SaveBindings();

        Debug.Log("Bindings reasignados y guardados.");
    }

    private void SaveBindings()
    {
        var playerMap = control.asset.FindActionMap("Player");
        foreach (string actionName in actionNames)
        {
            var action = playerMap.FindAction(actionName);
            for (int i = 0; i < action.bindings.Count; i++)
            {
                PlayerPrefs.SetString($"{actionName}_binding_{i}", action.bindings[i].effectivePath);
            }
        }
        PlayerPrefs.Save();
        Debug.Log("Bindings guardados.");
    }

    private void LoadBindings()
    {
        var playerMap = control.asset.FindActionMap("Player");
        foreach (string actionName in actionNames)
        {
            var action = playerMap.FindAction(actionName);
            for (int i = 0; i < action.bindings.Count; i++)
            {
                string key = $"{actionName}_binding_{i}";
                if (PlayerPrefs.HasKey(key))
                {
                    action.ApplyBindingOverride(i, PlayerPrefs.GetString(key));
                }
            }
        }
        Debug.Log("Bindings cargados.");
    }

    private void SetDropdownsToCurrentBindings()
    {
        // Para cada dropdown, establecer el valor según el binding actual
        var playerMap = control.asset.FindActionMap("Player");
        for (int i = 0; i < dropdowns.Length; i++)
        {
            string actionName = actionNames[i];
            var action = playerMap.FindAction(actionName);
            if (action == null) continue;

            // Obtenemos el binding actual y buscamos a qué acción original pertenece
            string currentBinding = action.bindings[0].effectivePath; // asumimos binding 0

            int index = 0; // default

            // Buscamos qué acción tiene ese binding en el original
            for (int j = 0; j < actionNames.Length; j++)
            {
                var otherAction = playerMap.FindAction(actionNames[j]);
                if (otherAction != null)
                {
                    for (int b = 0; b < otherAction.bindings.Count; b++)
                    {
                        if (otherAction.bindings[b].effectivePath == currentBinding)
                        {
                            index = j;
                            break;
                        }
                    }
                }
            }
            dropdowns[i].value = index;
        }
    }
}
