using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character 
{
    public PlayerMovementController movementController;

    // Start is called before the first frame update
    void Start()
    {
        characterHealth.maxHealth = 100;
        characterHealth.minHealth = 0;
        characterStamina.maxStamina = 100;
        characterStamina.minStamina = 0;
        movementController.setSpeed(walkSpeed);
        movementController.setSprintSpeedMult(sprintSpeedMult);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            characterHealth.takeDamage(25);
        } else if (Input.GetKeyDown(KeyCode.C))
        {
            characterHealth.giveHealth(10);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift)) startSprinting();
        if (Input.GetKeyUp(KeyCode.LeftShift) || !characterStamina.hasStamina) stopSprinting();
    }

    private void startSprinting()
    {
        characterStamina.startSprinting();
        movementController.startSprinting();
    }

    private void stopSprinting()
    {
        characterStamina.stopSprinting();
        movementController.stopSprinting();
    }
}
