using UnityEngine;

public enum MainStatType { Strength, Agility, Vitality }

public class Stats : MonoBehaviour
{
    public int strength;
    public int agility;
    public int vitality;

    private const int MAX_STRENGTH = 100;
    private const int MAX_AGILITY = 100;
    private const int MAX_VITALITY = 100;

    private const int MIN_STRENGTH = 0;
    private const int MIN_AGILITY = 0;
    private const int MIN_VITALITY = 0;

    public int[] damageRange = new int[2];
    public int[] attackRange = new int[2];
    public int[] defenseRange = new int[2];

    private int[] damageRangePerLevel = { 0, 3 };
    private int[] attackRangePerLevel = { 0, 3 };
    private int[] defenseRangePerLevel = { 0, 3 };

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

    public void SetupStats(int initStrength, int initAgility, int initVitality)
    {
        SetStrength(initStrength);
        SetAgility(initAgility);
        SetVitality(initVitality);
    }

    public void UpdateStats()
    {
        damageRange = (new int[2] { strength * damageRangePerLevel[0], strength * damageRangePerLevel[1] });
        attackRange = (new int[2] { agility * attackRangePerLevel[0], agility * attackRangePerLevel[1] });
        defenseRange = (new int[2] { agility * defenseRangePerLevel[0], agility * defenseRangePerLevel[1] });
    }

    public void SetStrength(int newStrength)
    {
        strength = newStrength;
        if (strength > MAX_STRENGTH) strength = MAX_STRENGTH;
        if (strength < MIN_STRENGTH) strength = MIN_STRENGTH;
    }

    public void SetAgility(int newAgility)
    {
        agility = newAgility;
        if (agility > MAX_AGILITY) agility = MAX_AGILITY;
        if (agility < MIN_AGILITY) agility = MIN_AGILITY;
    }

    public void SetVitality(int newVitality)
    {
        vitality = newVitality;
        if (vitality > MAX_VITALITY) vitality = MAX_VITALITY;
        if (vitality < MIN_VITALITY) vitality = MIN_VITALITY;
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
