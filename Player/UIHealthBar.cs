using UnityEngine;
using UnityEngine.UI;
public class UIHealthBar : MonoBehaviour
{
    public Slider slider;

    public void SetSliderValue(float value)
    {
        slider.value = value;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
