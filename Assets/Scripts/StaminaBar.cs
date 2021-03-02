using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{

    public Slider slider;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    { }

    // Resets all values to source
    public void Reset(Stamina staminaSource)
    {
        slider.minValue = staminaSource.minStamina;
        slider.maxValue = staminaSource.maxStamina;
        Refresh(staminaSource);
    }

    // Resets only the current value to source
    public void Refresh(Stamina staminaSource)
    {
        slider.value = staminaSource.currentStamina;
    }
}
