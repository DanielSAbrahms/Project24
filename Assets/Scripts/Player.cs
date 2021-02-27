using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Health playerHealth;
    public PlayerMovementController movementController;

    public int walkSpeed = 40;
    public int sprintSpeed = 60;
    public enum Speed { Walk, Sprint};
    public Speed currentSpeed;


    // Start is called before the first frame update
    void Start()
    {
        playerHealth.maxHealth = 100;
        playerHealth.minHealth = 0;

        Speed currentSpeed = Speed.Walk;
        //movementController.setSpeed(currentSpeed == Speed.Walk ? walkSpeed : sprintSpeed);
        DumpToConsole(currentSpeed == Speed.Walk ? walkSpeed : sprintSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            playerHealth.takeDamage(10);
        } else if (Input.GetKeyDown(KeyCode.C))
        {
            playerHealth.giveHealth(10);
        }


        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            currentSpeed = Speed.Sprint;
        } else
        {
            currentSpeed = Speed.Walk;
        }

        //movementController.setSpeed(currentSpeed == Speed.Walk ? walkSpeed : sprintSpeed);

        DebugAll();
    }

    // --------------------- Debug ------------------------------------

    public static void DumpToConsole(object obj)
    {
        var output = JsonUtility.ToJson(obj, true);
        Debug.Log(output);
    }
    public void DebugAll()
    {
        // DumpToConsole(movementController.speed);
    }
}
