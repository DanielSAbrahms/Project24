using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MainStatType { Strength, Agility, Vitality }

public class Stats : MonoBehaviour
{
    public int strength;
    public int agility;
    public int vitality;

    private const int maxStrength = 100;
    private const int maxAgility = 100;
    private const int maxVitality = 100;

    private const int minStrength = 0;
    private const int minAgility = 0;
    private const int minVitality = 0;

    public int[] damageRange = new int[2];
    public int[] attackRange = new int[2];
    public int[] defenseRange = new int[2];

    private int[] damageRangePerLevel = { 0, 3 };
    private int[] attackRangePerLevel = { 0, 3 };
    private int[] defenseRangePerLevel = { 0, 3 };

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addOneToMainStat(MainStatType statType)
    {
        switch( statType)
        {
            case MainStatType.Strength:
                strength++;
                break;
            case MainStatType.Agility:
                agility++;
                break;
            case MainStatType.Vitality:
                vitality++;
                break;
        }
    }

    public void setupStats(int initStrength, int initAgility, int initVitality)
    {
        setStrength(initStrength);
        setAgility(initAgility);
        setVitality(initVitality);
    }

    public void updateStats()
    {
        damageRange = (new int[2] { strength * damageRangePerLevel[0], strength * damageRangePerLevel[1] });
        attackRange = (new int[2] { agility * attackRangePerLevel[0], agility * attackRangePerLevel[1] });
        defenseRange = (new int[2] { agility * defenseRangePerLevel[0], agility * defenseRangePerLevel[1] });
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

    public void setVitality(int newVitality)
    {
        vitality = newVitality;
        if (vitality > maxVitality) vitality = maxVitality;
        if (vitality < minVitality) vitality = minVitality;
    }

    public int getRandomDamage()
    {
        return getRandomFromRange(damageRange);
    }

    private int getRandomFromRange(int[] range)
    {
        return Random.Range(range[0], range[1]);
    }
}
