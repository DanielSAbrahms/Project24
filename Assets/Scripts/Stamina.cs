using UnityEngine;

public class Stamina : MonoBehaviour
{
    public int maxStamina;
    public int minStamina = 0;
    public int currentStamina;
    public bool hasStamina;

    private const float STAMINA_SPRINT_USAGE_RATE = 0.03f;
    private float staminaSprintCache = 0f;
    private const int STAMINA_SPRINT_CACHE_LIMIT = 1;

    private const float STAMINA_REGEN_RATE = 0.01f;
    private float staminaRegenCache = 0f;
    private const int STAMINA_REGEN_CACHE_LIMIT = 1;

    private bool isSprinting;

    public StaminaBar staminaBar;

    // Start is called before the first frame update
    void Start()
    {
        currentStamina = maxStamina;
        hasStamina = true;
        staminaBar.slider.maxValue = maxStamina;
        staminaBar.slider.minValue = minStamina;

        isSprinting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSprinting) UseStaminaForFrame();
        else RegenStaminaForFrame();
        staminaBar.slider.value = currentStamina;
    }

    public void GiveStamina(int newStamina)
    {
        updateStamina(currentStamina + newStamina);
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
    private void UseStaminaForFrame()
    {
        staminaSprintCache += STAMINA_SPRINT_USAGE_RATE;
        if (staminaSprintCache >= STAMINA_SPRINT_CACHE_LIMIT)
        {
            staminaSprintCache = 0f;
            updateStamina(currentStamina - STAMINA_SPRINT_CACHE_LIMIT);
        }
    }

    // Handles Regen every frame
    private void RegenStaminaForFrame()
    {
        staminaRegenCache += STAMINA_REGEN_RATE;
        if (staminaRegenCache >= STAMINA_REGEN_CACHE_LIMIT)
        {
            staminaRegenCache = 0f;
            updateStamina(currentStamina + STAMINA_REGEN_CACHE_LIMIT);
        }
    }

    // Handles updating stamina
    private void updateStamina(int newStamina)
    {
        currentStamina = newStamina;

        if (currentStamina > maxStamina) currentStamina = maxStamina;

        if (currentStamina <= 0)
        {
            hasStamina = false;
            currentStamina = minStamina;
        } else
        {
            hasStamina = true;
        }
    }
}
