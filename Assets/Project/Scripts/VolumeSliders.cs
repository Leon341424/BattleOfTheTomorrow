using System;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSliders : MonoBehaviour
{
    private string fileName = "audioSettings.json";
    public Slider musicVolume;
    public Slider SFxVolume;
    public AudioMixer globalAudioMixer;

    private AudioVolumes tmpVolumes;

    void Start()
    {
        LoadVolumes();
    }

    private void OnEnable()
    {
        musicVolume.onValueChanged.AddListener(OnMusicVolumeChange);
        SFxVolume.onValueChanged.AddListener(OnSFxVolumeChange);
    }

    private void OnDisable()
    {
        musicVolume.onValueChanged.RemoveListener(OnMusicVolumeChange);
        SFxVolume.onValueChanged.RemoveListener(OnSFxVolumeChange);
        SaveVolumes(); 
    }

    public void OnMusicVolumeChange(float volume)
    {
        globalAudioMixer.SetFloat("MusicVolume", volume);
        tmpVolumes.music = volume;
    }

    public void OnSFxVolumeChange(float volume)
    {
        globalAudioMixer.SetFloat("SFxVolume", volume);
        tmpVolumes.SFx = volume;
    }

    private void SaveVolumes()
    {
        string fullPath = Path.Combine(Application.persistentDataPath, fileName);
        string tmpJson = JsonUtility.ToJson(tmpVolumes);
        File.WriteAllText(fullPath, tmpJson);
    }

    private void LoadVolumes()
    {
        string fullPath = Path.Combine(Application.persistentDataPath, fileName);

        if (File.Exists(fullPath))
        {
            string volumeContent = File.ReadAllText(fullPath);
            tmpVolumes = JsonUtility.FromJson<AudioVolumes>(volumeContent);
        }
        else
        {
            tmpVolumes = new AudioVolumes();
        }

        SFxVolume.value = tmpVolumes.SFx;
        globalAudioMixer.SetFloat("SFxVolume", tmpVolumes.SFx);

        musicVolume.value = tmpVolumes.music;
        globalAudioMixer.SetFloat("MusicVolume", tmpVolumes.music);
    }
}

[Serializable]
public class AudioVolumes
{
    public float music;
    public float SFx;

    public AudioVolumes()
    {
        music = 0f;
        SFx = 0f;
    }
}