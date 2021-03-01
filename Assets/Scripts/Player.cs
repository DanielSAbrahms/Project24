using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character 
{
    public EXP characterEXP;
    public PlayerMovementController movementController;
    public StatMenuManager statMenuManager;
    private const int requiredEXPPerLevel = 1000;
    private const int levelUpPointsEachLevel = 5;


    // Start is called before the first frame update
    void Start()
    {
        level = 1;
        stats.setupStats(10, 10, 10);
        characterEXP.InitMaxEXP(requiredEXPPerLevel);
        characterHealth.minHealth = 0;
        characterStamina.minStamina = 0;
        movementController.setSpeed(walkSpeed);
        movementController.setSprintSpeedMult(sprintSpeedMult);

        updateStats();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            characterHealth.takeDamage(25);
        } else if (Input.GetKeyDown(KeyCode.C))
        {
            characterHealth.giveHealth(10);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!statMenuManager.IsMenuOpen()) { statMenuManager.OpenMenu(level, stats); }
            else { statMenuManager.CloseMenu(); }

        }

        if (Input.GetKeyDown(KeyCode.L)) giveEXP(100);

        if (Input.GetKeyDown(KeyCode.LeftShift)) startSprinting();
        if (Input.GetKeyUp(KeyCode.LeftShift) || !characterStamina.hasStamina) stopSprinting();
    }
    
    void levelUp()
    {
        int newMaxEXP = level * requiredEXPPerLevel;
        int excessEXP = characterEXP.levelUp(newMaxEXP);
        StartCoroutine(statMenuManager.WaitForLevelUpSelect(levelUpPointsEachLevel, (MainStatType statType) => {
            stats.addOneToMainStat(statType);
            statMenuManager.Refresh(level, stats);
        }));
        if (excessEXP > 0) giveEXP(excessEXP);
        updateStats();
    }

    void giveEXP(int newEXP)
    {
        characterEXP.giveEXP(newEXP);
        if (characterEXP.isReadyToLevelUp)
        {
            levelUp();
        }
    }

    private void startSprinting()
    {
        characterStamina.startSprinting();
        movementController.startSprinting();
    }

    private void stopSprinting()
    {
        characterStamina.stopSprinting();
        movementController.stopSprinting();
    }
}
