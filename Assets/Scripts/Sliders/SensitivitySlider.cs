using UnityEngine;
using UnityEngine.UI;

public class SensitivitySlider : MonoBehaviour
{
    public Slider slider;
    public PlayerController player;

    private void Awake()
    {
        slider.value = PlayerPrefs.GetFloat(PlayerPrefsKeys.sensitivitySliderKey, PlayerPrefsKeys.sensitivitySliderKeyDefaultValue);
    }

    void Start()
    {
        slider.onValueChanged.AddListener((v) =>
        {
            player.SetSensitivity(v);
            PlayerPrefs.SetFloat(PlayerPrefsKeys.sensitivitySliderKey, v);
        });
    }
}
