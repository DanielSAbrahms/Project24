using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth;
    public int minHealth = 0;
    public int currentHealth;
    public bool hasHealth;

    public HealthBar healthBar;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        hasHealth = true;
        ResetHealthBar();
    }

    // Update is called once per frame
    void Update()
    {
        RefreshHealthBar();
    }

    public void ResetHealthBar()
    {
        healthBar.Reset(this);
    }

    public void RefreshHealthBar()
    {
        healthBar.Refresh(this);
    }

    public void GiveHealth (int newHealth)
    {
        UpdateHealth(currentHealth + newHealth);
    }

    public void TakeDamage(int incomingDamage)
    {
        UpdateHealth(currentHealth - incomingDamage);
    }

    private void UpdateHealth(int newHealth)
    {
        currentHealth = newHealth;
        if (currentHealth > maxHealth) currentHealth = maxHealth;

        if (currentHealth <= 0)
        {
            hasHealth = false;
            currentHealth = minHealth;
        }
    }
}
