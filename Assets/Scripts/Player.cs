using UnityEngine;

public class Player : Character
{
    public EXP playerEXP;
    public PlayerMovementController movementController;
    public StatMenuManager statMenuManager;
    CharacterManager characterManager;

    public HUD playerHUD;

    // Start is called before the first frame update
    void Start()
    {
        playerHUD = GameObject.FindObjectOfType<HUD>();

        level = 1;
        affiliation = 0; // Player Team
        stats.SetupStats(Parameters.DEFAULT_STRENGTH, Parameters.DEFAULT_AGILITY, Parameters.DEFAULT_VITALITY);
        playerEXP.InitMaxEXP(Parameters.REQUIRED_EXP_PER_LEVEL);

        InitPlayerStats();
        playerHUD.Reset(this);
        playerHUD.RemoveCurrentEnemy();

        characterManager = GameObject.FindObjectOfType<CharacterManager>();

        if (!characterManager.characters.Contains(this))
        {
            characterManager.characters.Add(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        playerHUD.RefreshHealthBar(characterHealth);

        if (Input.GetKeyDown(KeyCode.X))
        {
            characterHealth.TakeDamage(25);
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            characterHealth.GiveHealth(10);
        }

        if (Input.GetKeyDown(Parameters.OPEN_STAT_MENU_KEY_BINDING))
        {
            if (!statMenuManager.IsMenuOpen()) { statMenuManager.OpenMenu(level, stats, characterHealth.maxHealth, characterStamina.maxStamina); }
            else { statMenuManager.CloseMenu(); }
        }

        if (Input.GetKeyDown(KeyCode.L)) GiveEXP(100000);

        if (Input.GetKeyDown(Parameters.SPRINT_KEY_BINDING)) StartSprinting();
        if (Input.GetKeyUp(Parameters.SPRINT_KEY_BINDING) || !characterStamina.hasStamina) StopSprinting();

        playerHUD.Refresh(this);
    }

    public void UpdatePlayerStats()
    {
        UpdateStats();
        characterHealth.maxHealth = (Parameters.PLAYER_DEFAULT_HEALTH + (stats.vitality * Parameters.MAX_HEALTH_SCALE));
        characterStamina.maxStamina = (Parameters.PLAYER_DEFAULT_STAMINA + (stats.vitality * Parameters.MAX_STAMINA_SCALE));
        playerHUD.UpdateAllStats(this);
    }

    public void InitPlayerStats()
    {
        // We need to update before calling inits for stats for the max to be set correctly
        UpdatePlayerStats();
        characterHealth.InitHealth();
        characterStamina.InitStamina();
    }

    public void LevelUp()
    {
        level++;
        statMenuManager.Reset(level, stats, characterHealth.maxHealth, characterStamina.maxStamina, Parameters.STATS_POINTS_EACH_LEVEL_UP);
        int newMaxEXP = level * Parameters.REQUIRED_EXP_PER_LEVEL;
        playerEXP.LevelUp(newMaxEXP);
        UpdatePlayerStats();
        StartCoroutine(statMenuManager.WaitForLevelUpSelect(Parameters.STATS_POINTS_EACH_LEVEL_UP, (MainStatType statType) =>
        {
            stats.AddOneToMainStat(statType);
            UpdatePlayerStats();
            statMenuManager.Reset(level, stats, characterHealth.maxHealth, characterStamina.maxStamina);
        }));
    }

    public void GiveEXP(int newEXP)
    {
        // I don't want the exp class to care about excess excess -> It'll just clip everything
        // I do want excess EXP to count towards the next level, so we handle that logic here
        // We keep adding excess points back and leveling up recursively if necessary
        // To the player, the excess level ups will appear stacked. But as the logic is identical, the resursive method's backwards order won't matter (I hope)
        int excessEXP = (playerEXP.currentEXP + newEXP) - playerEXP.maxEXP;
        playerEXP.GiveEXP(newEXP);
        if (playerEXP.isReadyToLevelUp)
        {
            LevelUp();
            if (excessEXP > 0) GiveEXP(excessEXP);
        }
    }

    public void OnAttacked(Enemy enemy)
    {
        playerHUD.SetCurrentEnemy(enemy);
    }

    public void OnForgetEnemy()
    {
        playerHUD.RemoveCurrentEnemy();
    }

    void OnDie()
    {
        // Unregister as an actor
        if (characterManager)
        {
            characterManager.characters.Remove(this);
        }
    }

    private void StartSprinting()
    {
        playerHUD.StartSprinting();
        movementController.StartSprinting();
    }

    private void StopSprinting()
    {
        playerHUD.StopSprinting();
        movementController.StopSprinting();
    }
}