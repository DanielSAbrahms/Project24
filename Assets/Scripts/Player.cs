using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public Health playerHealth;

    // Start is called before the first frame update
    void Start()
    {
        playerHealth.maxHealth = 100;
        playerHealth.minHealth = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
