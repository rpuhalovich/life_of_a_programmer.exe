using UnityEngine;
using UnityEngine.UI;

// Surely there's a way to combine these scripts.
// The OnValueChanged inside the slider seems to be broken.
public class MusicVolumeSlider : MonoBehaviour
{
    public Slider slider;

    public GameObject musicObject;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = musicObject.GetComponent<AudioSource>();

        // Get saved volume, return 0.1 (PlayerPrefsKeys.musicVolumeKeyDefaultValue) if it hasn't been saved yet.
        audioSource.volume = PlayerPrefs.GetFloat(PlayerPrefsKeys.musicVolumeKey, PlayerPrefsKeys.musicVolumeKeyDefaultValue);
        slider.value = PlayerPrefs.GetFloat(PlayerPrefsKeys.musicVolumeKey, PlayerPrefsKeys.musicVolumeKeyDefaultValue);
    }

    void Start()
    {
        slider.onValueChanged.AddListener((v) => {
            audioSource.volume = v;
            PlayerPrefs.SetFloat(PlayerPrefsKeys.musicVolumeKey, v);
        });
    }
}
