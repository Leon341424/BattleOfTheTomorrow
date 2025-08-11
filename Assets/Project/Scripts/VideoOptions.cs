using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VideoOptions : MonoBehaviour
{
    public Slider brightnessSlider;
    public Image blackOverlay; 
    private const string BrightnessKey = "BrightnessValue";

    public TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;
    void Start()
    {
        float savedBrightness = PlayerPrefs.GetFloat(BrightnessKey, 1f);
        brightnessSlider.value = savedBrightness;
        SetBrightness(savedBrightness);

        brightnessSlider.onValueChanged.AddListener(value =>
        {
            SetBrightness(value);
            SaveBrightness(value);
        });  

        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        var options = new System.Collections.Generic.List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        resolutionDropdown.onValueChanged.AddListener(SetResolution);
    }

    void SetBrightness(float value)
    {
        blackOverlay.color = new Color(0, 0, 0, 1 - value * 0.5f);
    }

    void SaveBrightness(float value)
    {
        PlayerPrefs.SetFloat(BrightnessKey, value);
        PlayerPrefs.Save();
    }

    public void SetResolution(int index)
    {
        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        //Debug.Log("Resolución actual: " + Screen.width + "x" + Screen.height);
    }
}
