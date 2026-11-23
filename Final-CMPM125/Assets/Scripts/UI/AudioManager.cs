using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public float Volume { get; private set; } // Stores the current volume level

    private void Awake()
    {
        // Prevent multiple AudioManagers
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Keep this object when changing scenes
        DontDestroyOnLoad(gameObject);

        // Load the saved volume
        Volume = PlayerPrefs.GetFloat("Volume", 1f);

        ApplyVolume();
    }

    public void SetVolume(float volume)
    {
        Volume = volume;

        // Save the volume setting
        PlayerPrefs.SetFloat("Volume", Volume);
        PlayerPrefs.Save();

        ApplyVolume(); 
    }

    private void ApplyVolume() // Applies the volume to the AudioListener
    {
        AudioListener.volume = Volume; 
    }
}