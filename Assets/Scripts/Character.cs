using UnityEngine;

public class Character : MonoBehaviour
{
    public int level;
    public Health characterHealth;
    public Stamina characterStamina;
    public Stats stats;

    public float walkSpeed; // How fast is walking
    public float sprintSpeedMult; // How much fast sprinting is than walking

    private const int MAX_HEALTH_PER_VITALITY_LEVEL = 10;
    private const int MAX_STAMINA_PER_VITALITY_LEVEL = 10;

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
        characterHealth.maxHealth = MAX_HEALTH_PER_VITALITY_LEVEL * stats.vitality;
        characterStamina.maxStamina = MAX_STAMINA_PER_VITALITY_LEVEL * stats.vitality;
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

    // --------------------- Debug ------------------------------------
    public static void DumpToConsole(object obj)
    {
        var output = JsonUtility.ToJson(obj, true);
        Debug.Log(output);
    }


}
