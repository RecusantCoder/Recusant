using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    private AudioManager audioManager;
    private Slider slider;
    public bool isMusicSlider;

    private void Start()
    {
        // Try to find the AudioManager in the scene
        audioManager = FindObjectOfType<AudioManager>();

        // Check if AudioManager was found
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found in the scene!");
            return;
        }

        // Get the Slider component attached to this GameObject
        slider = GetComponent<Slider>();

        // Check if Slider component was found
        if (slider == null)
        {
            Debug.LogError("Slider component not found on this GameObject!");
            return;
        }

        // Set up the On Value Changed callback
        slider.onValueChanged.AddListener(delegate { OnSliderValueChanged(); });
    }

    // Callback method for the slider value changed event
    private void OnSliderValueChanged()
    {
        if (isMusicSlider)
        {
            audioManager.SetMusicVolume(slider.value);
        }
        else
        {
            audioManager.SetSoundVolume(slider.value);
        }
    }
}
