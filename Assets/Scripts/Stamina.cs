using System;
using UnityEngine;

public class Stamina : MonoBehaviour
{
    public int maxStamina;
    public int minStamina = 0;
    public int currentStamina;
    public bool hasStamina;


    // Start is called before the first frame update
    void Start()
    {
        InitStamina();
    }

    public void GiveStamina(int newStamina)
    {
        updateStamina(currentStamina + newStamina);
    }

    public void UseStamina(int usedStamina)
    {
        updateStamina(currentStamina - usedStamina);
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

    public void InitStamina()
    {
        currentStamina = maxStamina;
        minStamina = 0;
        hasStamina = true;
    }
}
