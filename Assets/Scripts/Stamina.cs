using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stamina : MonoBehaviour
{
    public int maxStamina;
    public int minStamina = 0;
    public int currentStamina;
    public bool hasStamina;

    public StaminaBar staminaBar;

    private const float staminaSprintRate = 0.03f;
    private float staminaSprintCache = 0f;
    private const int staminaSprintCacheLimit = 1;

    private const float staminaRegenRate = 0.01f;
    private float staminaRegenCache = 0f;
    private const int staminaRegenCacheLimit = 1;

    private bool isSprinting;

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
        if (isSprinting) useStaminaForFrame();
        else regenStaminaForFrame();
        staminaBar.slider.value = currentStamina;
    }

    public void giveStamina(int newStamina)
    {
        updateStamina(currentStamina + newStamina);
    }

    public void startSprinting()
    {
        isSprinting = true;
    }

    public void stopSprinting()
    {
        isSprinting = false;
    }

    // Handles usage every frame (sprinting)
    private void useStaminaForFrame()
    {
        staminaSprintCache += staminaSprintRate;
        if (staminaSprintCache >= staminaSprintCacheLimit)
        {
            staminaSprintCache = 0f;
            updateStamina(currentStamina - staminaSprintCacheLimit);
        }
    }

    // Handles Regen every frame
    private void regenStaminaForFrame()
    {
        staminaRegenCache += staminaRegenRate;
        if (staminaRegenCache >= staminaRegenCacheLimit)
        {
            staminaRegenCache = 0f;
            updateStamina(currentStamina + staminaRegenCacheLimit);
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
