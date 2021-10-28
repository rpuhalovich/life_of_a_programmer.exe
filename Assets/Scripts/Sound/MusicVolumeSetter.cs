using UnityEngine;

public class MusicVolumeSetter : MonoBehaviour
{
    public AudioSource audioSource;

    private void Start()
    {
        audioSource.volume = PlayerPrefs.GetFloat(PlayerPrefsKeys.musicVolumeKey, PlayerPrefsKeys.musicVolumeKeyDefaultValue);
    }
}
