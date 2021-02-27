using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth;
    public int minHealth = 0;
    public int currentHealth;
    public bool hasHealth;

    private HealthBar healthBar;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        hasHealth = true;
        healthBar.slider.maxValue = maxHealth;
        healthBar.slider.minValue = minHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void giveHealth (int newHealth)
    {
        updateHealth(currentHealth + newHealth);
    }

    public void takeDamage(int incomingDamage)
    {
        updateHealth(currentHealth - incomingDamage);
    }

    private void updateHealth(int newHealth)
    {
        currentHealth = newHealth;

        int surplusHealth = currentHealth - maxHealth;
        if (surplusHealth > 0) currentHealth -= surplusHealth;

        if (currentHealth <= 0)
        {
            hasHealth = false;
            currentHealth = minHealth;
        }
    }
}
