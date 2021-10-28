using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        slider.onValueChanged.AddListener()
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
