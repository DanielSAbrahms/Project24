using UnityEngine;

public class HUD : MonoBehaviour
{
    public HealthBar healthBar;
    public StaminaBar staminaBar;
    public EXPBar expBar;

    private bool isSprinting;

    // Stamina Sprint Usage/ Regen
    private float staminaSprintCache = 0f;
    private float staminaRegenCache = 0f;

    void Start()
    {
        healthBar = GameObject.FindObjectOfType<HealthBar>();
        staminaBar = GameObject.FindObjectOfType<StaminaBar>();
        expBar = GameObject.FindObjectOfType<EXPBar>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateAllStats(Player player)
    {
        ResetStaminaBar(player.characterStamina);
        ResetHealthBar(player.characterHealth);
        ResetEXPBar(player.playerEXP);
    }

    public void Refresh(Player player)
    {
        if (isSprinting) UseStaminaForFrame(player.characterStamina);
        else RegenStaminaForFrame(player.characterStamina);
        RefreshStaminaBar(player.characterStamina);
        RefreshHealthBar(player.characterHealth);
        RefreshEXPBar(player.playerEXP);
    }


    public void ResetHealthBar(Health health)
    {
        healthBar.Reset(health);
    }

    public void RefreshHealthBar(Health health)
    {
        healthBar.Refresh(health);
    }

    public void ResetStaminaBar(Stamina stamina)
    {
        staminaBar.Reset(stamina);
    }

    public void RefreshStaminaBar(Stamina stamina)
    {
        staminaBar.Refresh(stamina);
    }

    public void StartSprinting()
    {
        isSprinting = true;
    }

    public void StopSprinting()
    {
        isSprinting = false;
    }

    // Handles usage every frame (sprinting)
    // Because we using ints for stamina, the smallest amount we can use is 1
    // If we use 1 stamina per frame, assuming were at 60fps, thats 60 stamina / second for sprinting (which is too much usage)
    // To solve this, I'll start with defining the usage per frame as a float less than 1
    // Also, I'm using a cache to keep the sum of the floats 
    // The cache builds up each frame, and when it reaches its limit (an int) we can subtract it from stamina
    // This means the stamina usage bar is slightly less than game fps 
    // It also means the sprint functionality is dependent on having a constant fps (bad)
    // I'm thinking I'll need to fix this using unity engine delays instead of the update method
    private void UseStaminaForFrame(Stamina playerStamina)
    {
        staminaSprintCache += Parameters.STAMINA_SPRINT_USAGE_RATE;
        if (staminaSprintCache >= Parameters.STAMINA_SPRINT_CACHE_LIMIT)
        {
            staminaSprintCache = 0f;
            playerStamina.UseStamina(Parameters.STAMINA_SPRINT_CACHE_LIMIT);
        }
    }

    // Handles Regen every frame
    private void RegenStaminaForFrame(Stamina playerStamina)
    {
        staminaRegenCache += Parameters.STAMINA_REGEN_RATE;
        if (staminaRegenCache >= Parameters.STAMINA_REGEN_CACHE_LIMIT)
        {
            staminaRegenCache = 0f;
            playerStamina.GiveStamina(Parameters.STAMINA_REGEN_CACHE_LIMIT);
        }
    }

    public void ResetEXPBar(EXP exp)
    {
        expBar.Reset(exp);
    }

    public void RefreshEXPBar(EXP exp)
    {
        expBar.Refresh(exp);
    }

}