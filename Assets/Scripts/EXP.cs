using System;
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
        ResetEXPBar();
    }

    // Update is called once per frame
    void Update()
    {
        RefreshEXPBar();
    }

    public void InitMaxEXP (int newMax)
    {
        maxEXP = newMax;
    }

    public void ResetEXPBar()
    {
        expBar.Reset(this);
    }

    public void RefreshEXPBar()
    {
        expBar.Refresh(this);
    }

    public void GiveEXP(int newEXP)
    {
        UpdateEXP(currentEXP + newEXP);
    }

    public void LevelUp(int newMax)
    {
        maxEXP = newMax;
        currentEXP = minEXP;
        ResetEXPBar();
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
