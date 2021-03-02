using System;
using UnityEngine;

public class Stamina : MonoBehaviour
{
    public int maxStamina;
    public int minStamina = 0;
    public int currentStamina;
    public bool hasStamina;
    public StaminaBar staminaBar;

    // Stamina Sprint Usage/ Regen
    private float staminaSprintCache = 0f;
    private float staminaRegenCache = 0f;

    private bool isSprinting;

    // Start is called before the first frame update
    void Start()
    {
        currentStamina = maxStamina;
        hasStamina = true;
        isSprinting = false;
        ResetStaminaBar();
    }

    // Update is called once per frame
    void Update()
    {
        if (isSprinting) UseStaminaForFrame();
        else RegenStaminaForFrame();
        RefreshStaminaBar();
    }

    public void ResetStaminaBar()
    {
        staminaBar.Reset(this);
    }

    public void RefreshStaminaBar()
    {
        staminaBar.Refresh(this);
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
        staminaSprintCache += Parameters.STAMINA_SPRINT_USAGE_RATE;
        if (staminaSprintCache >= Parameters.STAMINA_SPRINT_CACHE_LIMIT)
        {
            staminaSprintCache = 0f;
            updateStamina(currentStamina - Parameters.STAMINA_SPRINT_CACHE_LIMIT);
        }
    }

    // Handles Regen every frame
    private void RegenStaminaForFrame()
    {
        staminaRegenCache += Parameters.STAMINA_REGEN_RATE;
        if (staminaRegenCache >= Parameters.STAMINA_REGEN_CACHE_LIMIT)
        {
            staminaRegenCache = 0f;
            updateStamina(currentStamina + Parameters.STAMINA_REGEN_CACHE_LIMIT);
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
