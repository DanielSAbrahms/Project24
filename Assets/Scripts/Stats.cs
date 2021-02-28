using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public int strength;
    public int agility;
    public int vitality;

    public int damage;
    public int attack;
    public int defense;

    private const int maxStrength = 100;
    private const int maxAgility = 100;
    private const int maxVitality = 100;

    private const int minStrength = 0;
    private const int minAgility = 0;
    private const int minVitality = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setStrength(int newStrength)
    {
        strength = newStrength;
        if (strength > maxStrength) strength = maxStrength;
        if (strength < minStrength) strength = minStrength;
    }

    public void setAgility(int newAgility)
    {
        agility = newAgility;
        if (agility > maxAgility) agility = maxAgility;
        if (agility < minAgility) agility = minAgility;
    }

    public void set(int newVitality)
    {
        vitality = newVitality;
        if (vitality > maxVitality) vitality = maxVitality;
        if (vitality < minVitality) vitality = minVitality;
    }
}
