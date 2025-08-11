using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class AudioClipEntry
{
    public string name;
    public AudioClip clip;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource audioSourceMusic;
    public AudioSource audioSourceSFX;

    public List<AudioClipEntry> musicClips = new List<AudioClipEntry>();
    public List<AudioClipEntry> sfxClips = new List<AudioClipEntry>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        DontDestroyOnLoad(gameObject);
    }
    public void Play(string name)
    {
        var c = musicClips.Find(clipEntry => clipEntry.name == name);
        if (c == null || c.clip == null)
        {
            Debug.LogWarning("Clip de música no encontrado: " + name);
            return;
        }
        audioSourceMusic.clip = c.clip;
        audioSourceMusic.Play();
    }

    public void PlayOneShot(string name)
    {
        var c = sfxClips.Find(clipEntry => clipEntry.name == name);
        if (c == null || c.clip == null)
        {
            Debug.LogWarning("Clip de SFX no encontrado: " + name);
            return;
        }
        audioSourceSFX.PlayOneShot(c.clip);
    }

    public void StopMusic()
    {
        audioSourceMusic.Stop();
    }
}