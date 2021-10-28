using UnityEngine;
using UnityEngine.UI;

public class SensitivitySlider : MonoBehaviour
{
    public Slider slider;
    public PlayerController player;

    void Start()
    {
        slider.onValueChanged.AddListener((v) => {
            //Debug.Log(v.ToString());
            player.SetSensitivity(v);
        });
    }
}
