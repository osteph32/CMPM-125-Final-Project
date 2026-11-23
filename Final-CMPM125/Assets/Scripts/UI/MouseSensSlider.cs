using UnityEngine;
using UnityEngine.UI;

public class MouseSensSlider : MonoBehaviour
{
    public Slider sensitivitySlider;

    private void Start()
    {   
         Debug.Log("Has saved sensitivity: " + PlayerPrefs.HasKey("MouseSensitivity"));
    Debug.Log("Saved sensitivity: " + PlayerPrefs.GetFloat("MouseSensitivity", CameraFollow.DefaultSensitivity));
        // Load the saved sensitivity into the slider
        sensitivitySlider.value = PlayerPrefs.GetFloat(
            "MouseSensitivity",
            CameraFollow.DefaultSensitivity
        );

        // Update sensitivity whenever the slider moves
        sensitivitySlider.onValueChanged.AddListener(SetSensitivity);

          // Save the initial slider value to PlayerPrefs
        SetSensitivity(sensitivitySlider.value);
    }

    private void SetSensitivity(float value)
    {
        // Save the new sensitivity
        PlayerPrefs.SetFloat("MouseSensitivity", value);
        PlayerPrefs.Save();
    }
}