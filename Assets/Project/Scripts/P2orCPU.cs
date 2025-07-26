using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class P2orCPU : MonoBehaviour
{
    public Toggle toggle;
    public Text label;

    void Start()
    {
        UpdateLabel(toggle.isOn);
        toggle.onValueChanged.AddListener(UpdateLabel);
    }

    void UpdateLabel(bool isPlayer2)
    {
        label.text = isPlayer2 ? "Player 2" : "   CPU";
    }
}
