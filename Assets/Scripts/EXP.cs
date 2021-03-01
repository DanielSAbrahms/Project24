using System.Collections;
using System.Collections.Generic;
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

    public void giveEXP(int newEXP)
    {
        updateEXP(currentEXP + newEXP);
    }

    public int levelUp(int newMax)
    {
        int excessEXP = currentEXP - maxEXP;
        maxEXP = newMax;
        currentEXP = minEXP;
        isReadyToLevelUp = false;
        return excessEXP;
    }

    // Handles updating stamina
    private void updateEXP(int newEXP)
    {
        currentEXP = newEXP;

        if (currentEXP >= maxEXP)
        {
            currentEXP = maxEXP;
            isReadyToLevelUp = true;
        } 

    }
}
