using UnityEngine;

public class Player : Character 
{
    public EXP characterEXP;
    public PlayerMovementController movementController;
    public StatMenuManager statMenuManager;

    // Start is called before the first frame update
    void Start()
    {
        level = 1;
        stats.SetupStats(10, 10, 10);
        characterEXP.InitMaxEXP(Parameters.REQUIRED_EXP_PER_LEVEL);
        characterHealth.minHealth = 0;
        characterStamina.minStamina = 0;

        UpdateStats();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            characterHealth.TakeDamage(25);
        } else if (Input.GetKeyDown(KeyCode.C))
        {
            characterHealth.GiveHealth(10);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!statMenuManager.IsMenuOpen()) { statMenuManager.OpenMenu(level, stats, characterHealth.maxHealth, characterStamina.maxStamina); }
            else { statMenuManager.CloseMenu(); }

        }

        if (Input.GetKeyDown(KeyCode.L)) GiveEXP(100);

        if (Input.GetKeyDown(KeyCode.LeftShift)) StartSprinting();
        if (Input.GetKeyUp(KeyCode.LeftShift) || !characterStamina.hasStamina) StopSprinting();
    }
    
    public void LevelUp()
    {
        level++;
        int newMaxEXP = level * Parameters.REQUIRED_EXP_PER_LEVEL;
        int excessEXP = characterEXP.LevelUp(newMaxEXP);
        if (excessEXP > 0) GiveEXP(excessEXP);
        UpdateStats();
        StartCoroutine(statMenuManager.WaitForLevelUpSelect(Parameters.STATS_POINTS_EACH_LEVEL_UP, (MainStatType statType) => {
            stats.AddOneToMainStat(statType);
            UpdateStats();
            statMenuManager.Refresh(level, stats, characterHealth.maxHealth, characterStamina.maxStamina);
        }));
    }

    public void GiveEXP(int newEXP)
    {
        characterEXP.GiveEXP(newEXP);
        if (characterEXP.isReadyToLevelUp)
        {
            LevelUp();
        }
    }

    private void StartSprinting()
    {
        characterStamina.StartSprinting();
        movementController.StartSprinting();
    }

    private void StopSprinting()
    {
        characterStamina.StopSprinting();
        movementController.StopSprinting();
    }
}
