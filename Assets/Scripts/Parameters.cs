using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parameters : MonoBehaviour
{
    // Gravity
    public const float GRAVITY = 14.0f;

    // Camera Settings
    public const float CAMERA_HEIGHT = 11.5f;
    public const float CAMERA_DISTANCE = 6f;

    // Default Main Stats
    public const int DEFAULT_STRENGTH = 10;
    public const int DEFAULT_AGILITY = 10;
    public const int DEFAULT_VITALITY = 10;

    // Default Health/ Stamina (Subtract DEFAULT_VITALITY * MAX_HEALTH/STAMINA_PER_LEVEL from Desired Number)
    public const int PLAYER_DEFAULT_HEALTH = 80;
    public const int PLAYER_DEFAULT_STAMINA = 80;

    // Movement Speed
    public const float PLAYER_WALK_SPEED = 5f; // How fast is walking
    public const float SPRINT_SPEED_MULTIPLIER = 1.33f; // How much fast sprinting is than walking

    // Level-Ups
    public const int REQUIRED_EXP_PER_LEVEL = 1000;
    public const int STATS_POINTS_EACH_LEVEL_UP = 5;

    // Min-Maxes for Main Stats
    public const int MIN_STRENGTH = 0;
    public const int MIN_AGILITY = 0;
    public const int MIN_VITALITY = 0;
    public const int MAX_STRENGTH = 150;
    public const int MAX_AGILITY = 150;
    public const int MAX_VITALITY = 150;

    // Other Stat Scale Settings
    public static readonly float[] DAMAGE_RANGE_PER_LEVEL = { 0.3f, 0.7f };
    public static readonly float[] ATTACK_RANGE_PER_LEVEL = { 0.27f, 0.75f };
    public static readonly float[] DEFENSE_RANGE_PER_LEVEL = { 0.36f, 0.63f };
    public const int MAX_HEALTH_PER_LEVEL = 3;
    public const int MAX_STAMINA_PER_LEVEL = 3;

    // Stamina Sprint Usage/ Regen
    public const int STAMINA_REGEN_CACHE_LIMIT = 1;
    public const float STAMINA_REGEN_RATE = 0.015f;
    public const float STAMINA_SPRINT_USAGE_RATE = 0.03f;
    public const int STAMINA_SPRINT_CACHE_LIMIT = 1;

    // Key Bindings
    // TODO: Find correct type for binding
    public const object OPEN_STAT_MENU_KEY_BINDING = KeyCode.P;
    public const object SPRINT_KEY_BINDING = KeyCode.LeftShift;

    // Clip excess movement
    public const float MAX_VELOCITY_CHANGE = 10.0f;

    // Start is called before the first frame update
    void Start()
    { }

    // Update is called once per frame
    void Update()
    { }
}
