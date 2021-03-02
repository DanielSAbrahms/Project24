using UnityEngine;

public class Character : MonoBehaviour
{
    public int level;
    public Health characterHealth;
    public Stamina characterStamina;
    public Stats stats;

    // Start is called before the first frame update
    void Start()
    {
        stats.SetupStats(10, 10, 10);
        characterHealth.maxHealth = 100;
        characterHealth.minHealth = 0;
        characterStamina.maxStamina = 100;
        characterStamina.minStamina = 0;
    }

    // Update is called once per frame
    void Update()
    {}

    // Updates characters health and stamina according to current stats
    public void UpdateStats()
    {
        characterHealth.maxHealth = Parameters.MAX_HEALTH_PER_LEVEL * stats.vitality;
        characterStamina.maxStamina = Parameters.MAX_STAMINA_PER_LEVEL * stats.vitality;
        stats.UpdateStats();
    }

    private void StartSprinting()
    {
        characterStamina.StartSprinting();
    }

    private void StopSprinting()
    {
        characterStamina.StopSprinting();
    }
}
