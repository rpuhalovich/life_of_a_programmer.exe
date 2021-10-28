using UnityEngine;
using UnityEngine.UI;

// Surely there's a way to combine these scripts.
// The OnValueChanged inside the slider seems to be broken.
public class MusicVolumeSlider : MonoBehaviour
{
    public Slider slider;

    public AudioManager audiomanager;
    public string songName;

    void Start()
    {
        slider.onValueChanged.AddListener((v) => {
            //Debug.Log(v.ToString());
            audiomanager.SetVolume(songName, v);
        });
    }
}
