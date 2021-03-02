using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
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
    public void Reset(Health healthSource)
    {
        slider.minValue = healthSource.minHealth;
        slider.maxValue = healthSource.maxHealth;
        Refresh(healthSource);
    }

    // Resets only the current value to source
    public void Refresh(Health healthSource)
    {
        slider.value = healthSource.currentHealth;
    }
}
