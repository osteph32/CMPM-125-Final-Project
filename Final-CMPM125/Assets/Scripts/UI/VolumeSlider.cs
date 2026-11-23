using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsUI : MonoBehaviour
{
    public Slider volumeSlider;

    private void Start()
    {
        // Set slider to the saved volume
        volumeSlider.value = AudioManager.Instance.Volume;

        // Update volume whenever the slider moves
        volumeSlider.onValueChanged.AddListener(
            AudioManager.Instance.SetVolume // Call the SetVolume method in AudioManager when the slider value changes
        );
    }
}