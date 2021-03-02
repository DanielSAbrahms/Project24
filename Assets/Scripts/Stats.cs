using UnityEngine;

public enum MainStatType { Strength, Agility, Vitality }

public class Stats : MonoBehaviour
{
    public int strength;
    public int agility;
    public int vitality;

    public int[] damageRange = new int[2];
    public int[] attackRange = new int[2];
    public int[] defenseRange = new int[2];

    // Start is called before the first frame update
    void Start()
    { }

    // Update is called once per frame
    void Update()
    { }

    public void AddOneToMainStat(MainStatType statType)
    {
        switch( statType)
        {
            case MainStatType.Strength:
                SetStrength(strength + 1);
                break;
            case MainStatType.Agility:
                SetAgility(agility + 1);
                break;
            case MainStatType.Vitality:
                SetVitality(vitality + 1);
                break;
        }
    }

    public void SetupStats(int initStrength, int initAgility, int initVitality)
    {
        SetStrength(initStrength);
        SetAgility(initAgility);
        SetVitality(initVitality);
    }

    public void UpdateStats()
    {
        damageRange = (new int[2] { (int)(strength * Parameters.DAMAGE_RANGE_PER_LEVEL[0]), (int)(strength * Parameters.DAMAGE_RANGE_PER_LEVEL[1]) });
        attackRange = (new int[2] { (int)(agility * Parameters.ATTACK_RANGE_PER_LEVEL[0]), (int)(agility * Parameters.ATTACK_RANGE_PER_LEVEL[1]) });
        defenseRange = (new int[2] { (int)(agility * Parameters.DEFENSE_RANGE_PER_LEVEL[0]), (int)(agility * Parameters.DEFENSE_RANGE_PER_LEVEL[1]) });
    }

    public void SetStrength(int newStrength)
    {
        strength = newStrength;
        if (strength > Parameters.MAX_STRENGTH) strength = Parameters.MAX_STRENGTH;
        if (strength < Parameters.MIN_STRENGTH) strength = Parameters.MIN_STRENGTH;
    }

    public void SetAgility(int newAgility)
    {
        agility = newAgility;
        if (agility > Parameters.MAX_AGILITY) agility = Parameters.MAX_AGILITY;
        if (agility < Parameters.MIN_AGILITY) agility = Parameters.MIN_AGILITY;
    }

    public void SetVitality(int newVitality)
    {
        vitality = newVitality;
        if (vitality > Parameters.MAX_VITALITY) vitality = Parameters.MAX_VITALITY;
        if (vitality < Parameters.MIN_VITALITY) vitality = Parameters.MIN_VITALITY;
    }

    public int GetRandomDamage()
    {
        return GetRandomFromRange(damageRange);
    }

    private int GetRandomFromRange(int[] range)
    {
        return Random.Range(range[0], range[1]);
    }
}
