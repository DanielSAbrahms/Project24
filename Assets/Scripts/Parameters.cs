using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parameters : MonoBehaviour
{
    // Gravity
    [Header("World Settings")]
    public const float GRAVITY = 14.0f;
    [Tooltip("Velocity Cap for player movement")]
    public const float MAX_VELOCITY_CHANGE = 10.0f;

    [Header("Camera Settings")]
    public const float CAMERA_HEIGHT = 11.5f;
    public const float CAMERA_DISTANCE = 6f;

    [Header("Default Main Stats")]
    public const int DEFAULT_STRENGTH = 10;
    public const int DEFAULT_AGILITY = 10;
    public const int DEFAULT_VITALITY = 10;
    [Tooltip("")]
    public const int PLAYER_DEFAULT_HEALTH = 80; // Extra health than vitality * MAX_HEALTH_SCALE
    [Tooltip("")]
    public const int PLAYER_DEFAULT_STAMINA = 80; // Extra stamina than vitality * MAX_STAMINA_SCALE

    [Header("Player Movement Settings")]
    [Tooltip("How fast is walking (ambiguous)")]
    public const float PLAYER_WALK_SPEED = 5f;
    [Tooltip("How much faster running is to walking ( > 1 )")]
    public const float SPRINT_SPEED_MULTIPLIER = 1.33f;

    [Header("Level Up Settings")]
    [Tooltip("Required EXP to reach for leveling up")]
    public const int REQUIRED_EXP_PER_LEVEL = 1000;
    [Tooltip("Number of stat points for the player to use when they level up")]
    public const int STATS_POINTS_EACH_LEVEL_UP = 5;

    [Header("Min/ Max of Main Stats")]
    public const int MIN_STRENGTH = 0;
    public const int MIN_AGILITY = 0;
    public const int MIN_VITALITY = 0;
    public const int MAX_STRENGTH = 150;
    public const int MAX_AGILITY = 150;
    public const int MAX_VITALITY = 150;

    [Header("Stat Scaling Rates")]
    [Tooltip("The range of damage values per character level")]
    public static readonly float[] DAMAGE_RANGE_SCALE = { 0.3f, 0.7f };
    [Tooltip("The range of attack values per character level")]
    public static readonly float[] ATTACK_RANGE_SCALE = { 0.27f, 0.75f };
    [Tooltip("The range of defense values per character level")]
    public static readonly float[] DEFENSE_RANGE_SCALE = { 0.36f, 0.63f };
    [Tooltip("Max health per character level")]
    public const int MAX_HEALTH_SCALE = 3;
    [Tooltip("Max stamina per character level")]
    public const int MAX_STAMINA_SCALE = 3;

    [Header("Player Sprinting Logic")]
    [Tooltip("Smallest Chunk of Stamina to Gain each Frame")]
    public const int STAMINA_REGEN_CACHE_LIMIT = 1;
    [Tooltip("How much stamina is gained per frame when not used")]
    public const float STAMINA_REGEN_RATE = 0.015f;
    [Tooltip("How much stamina is used per frame when spriting")]
    public const int STAMINA_SPRINT_CACHE_LIMIT = 1;
    [Tooltip("Smallest Chunk of Stamina to Use each Frame")]
    public const float STAMINA_SPRINT_USAGE_RATE = 0.03f;

    [Header("Keybinds")]
    [Tooltip("Open Stat Menu Keybind")]
    public const UnityEngine.KeyCode OPEN_STAT_MENU_KEY_BINDING = KeyCode.P;
    [Tooltip("Sprinting Keybind (Hold)")]
    public const UnityEngine.KeyCode SPRINT_KEY_BINDING = KeyCode.LeftShift;

    [Header("Enemy Generation Settings")]
    public static readonly float[] ENEMY_LEVEL_RANGE_SCALE = { 0.2f, 1.3f };

    [Header("Enemy Logic Settings")]
    [Tooltip("The Y height at which the enemy will be automatically killed (if it falls off of the level)")]
    public const float selfDestructYHeight = -20f;
    [Tooltip("The distance at which the enemy considers that it has reached its current path destination point")]
    public const float pathReachingRadius = 2f;
    [Tooltip("The speed at which the enemy rotates")]
    public const float orientationSpeed = 10f;
    [Tooltip("Delay after death where the GameObject is destroyed (to allow for animation)")]
    public const float deathDuration = 0f;
}
