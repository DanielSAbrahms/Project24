using UnityEngine;

public class Character : MonoBehaviour
{
    public int level;
    public Health characterHealth;
    public Stamina characterStamina;
    public Stats stats;

    [Tooltip("Represents the affiliation (or team) of the actor. Actors of the same affiliation are friendly to eachother")]
    public int affiliation;
    [Tooltip("Represents point where other actors will aim when they attack this actor")]
    public Transform aimPoint;

    CharacterManager characterManager;

    // Start is called before the first frame update
    void Start()
    {
        stats.SetupStats(10, 10, 10);
        characterHealth.maxHealth = 100;
        characterHealth.minHealth = 0;
        characterStamina.maxStamina = 100;
        characterStamina.minStamina = 0;

        characterManager = GameObject.FindObjectOfType<CharacterManager>();

        if (!characterManager.characters.Contains(this))
        {
            characterManager.characters.Add(this);
        }
    }

    private void OnDestroy()
    {
        // Unregister as an actor
        if (characterManager)
        {
            characterManager.characters.Remove(this);
        }
    }

    // Updates characters health and stamina according to current stats
    public void UpdateStats()
    {
        characterHealth.maxHealth = Parameters.MAX_HEALTH_SCALE * stats.vitality;
        characterStamina.maxStamina = Parameters.MAX_STAMINA_SCALE * stats.vitality;
        stats.UpdateStats();
    }
}
