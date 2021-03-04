using System;
using UnityEngine;

public class EXP : MonoBehaviour
{
    public int maxEXP;
    public int minEXP = 0;
    public int currentEXP;
    public bool isReadyToLevelUp;

    void Start()
    {
        currentEXP = minEXP;
        isReadyToLevelUp = false;
    }

    public void InitMaxEXP (int newMax)
    {
        maxEXP = newMax;
    }

    public void GiveEXP(int newEXP)
    {
        UpdateEXP(currentEXP + newEXP);
    }

    public void LevelUp(int newMax)
    {
        maxEXP = newMax;
        currentEXP = minEXP;
        isReadyToLevelUp = false;
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
