using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parameters : MonoBehaviour
{
    // Gravity
    public const float GRAVITY = 14.0f;

    // Camera Settings
    public const float CAMERA_HEIGHT = 13f;
    public const float CAMERA_DISTANCE = 7f;

    // Movement Speed
    public const float PLAYER_WALK_SPEED = 1f; // How fast is walking
    public const float SPRINT_SPEED_MULTIPLIER = 1.5f; // How much fast sprinting is than walking

    // Level-Ups
    public const int REQUIRED_EXP_PER_LEVEL = 1000;
    public const int STATS_POINTS_EACH_LEVEL_UP = 5;

    // Min-Maxes for Main Stats
    public const int MIN_STRENGTH = 0;
    public const int MIN_AGILITY = 0;
    public const int MIN_VITALITY = 0;
    public const int MAX_STRENGTH = 100;
    public const int MAX_AGILITY = 100;
    public const int MAX_VITALITY = 100;

    // Other Stat Scale Settings
    public static readonly float[] DAMAGE_RANGE_PER_LEVEL = { 0f, 1.2f };
    public static readonly float[] ATTACK_RANGE_PER_LEVEL = { 0f, 1.2f };
    public static readonly float[] DEFENSE_RANGE_PER_LEVEL = { 0f, 1.2f };
    public const int MAX_HEALTH_PER_LEVEL = 10;
    public const int MAX_STAMINA_PER_LEVEL = 10;

    // Stamina Sprint Usage/ Regen
    public const int STAMINA_REGEN_CACHE_LIMIT = 1;
    public const float STAMINA_REGEN_RATE = 0.01f;
    public const float STAMINA_SPRINT_USAGE_RATE = 0.03f;
    public const int STAMINA_SPRINT_CACHE_LIMIT = 1;

    // Clip excess movement
    public const float MAX_VELOCITY_CHANGE = 10.0f;

    // Start is called before the first frame update
    void Start()
    { }

    // Update is called once per frame
    void Update()
    { }
}
