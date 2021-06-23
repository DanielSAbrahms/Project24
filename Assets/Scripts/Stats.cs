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
        damageRange = (new int[2] { Mathf.RoundToInt(strength * Parameters.DAMAGE_RANGE_SCALE[0]), Mathf.RoundToInt(strength * Parameters.DAMAGE_RANGE_SCALE[1]) });
        attackRange = (new int[2] { Mathf.RoundToInt(agility * Parameters.ATTACK_RANGE_SCALE[0]), Mathf.RoundToInt(agility * Parameters.ATTACK_RANGE_SCALE[1]) });
        defenseRange = (new int[2] { Mathf.RoundToInt(agility * Parameters.DEFENSE_RANGE_SCALE[0]), Mathf.RoundToInt(agility * Parameters.DEFENSE_RANGE_SCALE[1]) });
        if (damageRange[0] == 0) damageRange[0] = 1;
        if (attackRange[0] == 0) attackRange[0] = 1;
        if (defenseRange[0] == 0) defenseRange[0] = 1;
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
        return Utilities.GetRandomFromRange(damageRange);
    }

    public int GetRandomAttack()
    {
        return Utilities.GetRandomFromRange(attackRange);
    }

    public int GetRandomDefense()
    {
        return Utilities.GetRandomFromRange(defenseRange);
    }
}
