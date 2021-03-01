using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public int level;
    public Health characterHealth;
    public Stamina characterStamina;
    public Stats stats;

    public float sprintSpeedMult;
    public float walkSpeed;

    private const int maxHealthPerVitalityLevel = 10;
    private const int maxStaminaPerVitalityLevel = 10;

    // Start is called before the first frame update
    void Start()
    {
        stats.setupStats(10, 10, 10);
        characterHealth.maxHealth = 100;
        characterHealth.minHealth = 0;
        characterStamina.maxStamina = 100;
        characterStamina.minStamina = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateStats()
    {
        characterHealth.maxHealth = maxHealthPerVitalityLevel * stats.vitality;
        characterStamina.maxStamina = maxStaminaPerVitalityLevel * stats.vitality;
        stats.updateStats();
    }

    private void startSprinting()
    {
        characterStamina.startSprinting();
    }

    private void stopSprinting()
    {
        characterStamina.stopSprinting();
    }

    // --------------------- Debug ------------------------------------
    public static void DumpToConsole(object obj)
    {
        var output = JsonUtility.ToJson(obj, true);
        Debug.Log(output);
    }


}
