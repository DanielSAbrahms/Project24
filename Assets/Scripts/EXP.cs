using UnityEngine;

public class EXP : MonoBehaviour
{
    public int maxEXP;
    public int minEXP = 0;
    public int currentEXP;
    public bool isReadyToLevelUp;
    public EXPBar expBar;

    // Start is called before the first frame update
    void Start()
    {
        currentEXP = minEXP;
        isReadyToLevelUp = false;
        expBar.slider.maxValue = maxEXP;
        expBar.slider.minValue = minEXP;
    }

    // Update is called once per frame
    void Update()
    {
        expBar.slider.value = currentEXP;
    }

    public void InitMaxEXP (int newMax)
    {
        maxEXP = newMax;
    }

    public void GiveEXP(int newEXP)
    {
        UpdateEXP(currentEXP + newEXP);
    }

    public int LevelUp(int newMax)
    {
        int excessEXP = currentEXP - maxEXP;
        maxEXP = newMax;
        expBar.slider.maxValue = maxEXP;
        currentEXP = minEXP;
        isReadyToLevelUp = false;
        return excessEXP;
    }

    // Handles updating exp
    private void UpdateEXP(int newEXP)
    {
        currentEXP = newEXP;

        if (currentEXP >= maxEXP)
        {
            currentEXP = maxEXP;
            isReadyToLevelUp = true;
        } 

    }
}
