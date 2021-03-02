using UnityEngine;
using UnityEngine.UI;

public class EXPBar : MonoBehaviour
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
    public void Reset(EXP expSource)
    {
        slider.minValue = expSource.minEXP;
        slider.maxValue = expSource.maxEXP;
        Refresh(expSource);
    }

    // Resets only the current value to source
    public void Refresh(EXP expSource) 
    {
        slider.value = expSource.currentEXP;
    }
}
