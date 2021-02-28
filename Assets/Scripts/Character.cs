using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Health characterHealth;
    public Stamina characterStamina;

    public float sprintSpeedMult;
    public float walkSpeed;

    // Start is called before the first frame update
    void Start()
    {
        characterHealth.maxHealth = 100;
        characterHealth.minHealth = 0;
        characterStamina.maxStamina = 100;
        characterStamina.minStamina = 0;
        // movementController.setSpeed(walkSpeed);
        // movementController.setSprintSpeedMult(sprintSpeedMult);
    }

    // Update is called once per frame
    void Update()
    {
        
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
