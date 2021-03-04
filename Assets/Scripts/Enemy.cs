using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public enum EnemyType { Default };

public class Enemy : Character
{
    private EnemyType enemyType;

    public void GenerateStats(int levelsCompleted)
    {
        int[] levelRange = { (int)Parameters.ENEMY_LEVEL_RANGE_SCALE[0] * levelsCompleted, (int)Parameters.ENEMY_LEVEL_RANGE_SCALE[1] * levelsCompleted };
        level = Utilities.GetRandomFromRange(levelRange);

        int numberOfEnemyTypes = Enum.GetNames(typeof(EnemyType)).Length;
        int[] typeNumRange = { 0, numberOfEnemyTypes };
        int enemyTypeNum = Utilities.GetRandomFromRange(typeNumRange);
        enemyType = (EnemyType)Enum.Parse(typeof(EnemyType), Enum.GetNames(typeof(EnemyType))[enemyTypeNum]);
    }

    [System.Serializable]
    public struct RendererIndexData
    {
        public Renderer renderer;
        public int materialIndex;

        public RendererIndexData(Renderer renderer, int index)
        {
            this.renderer = renderer;
            this.materialIndex = index;
        }
    }


    //[Header("Weapons Parameters")]
    //[Tooltip("Allow weapon swapping for this enemy")]
    //public bool swapToNextWeapon = false;
    //[Tooltip("Time delay between a weapon swap and the next attack")]
    //public float delayAfterWeaponSwap = 0f;

    [Tooltip("The gradient representing the color of the flash on hit")]
    [GradientUsageAttribute(true)]
    public Gradient onHitBodyGradient;

    [Header("VFX")]
    [Tooltip("The VFX prefab spawned when the enemy dies")]
    public GameObject deathVFX;
    [Tooltip("The point at which the death VFX is spawned")]
    public Transform deathVFXSpawnPoint;

    //[Header("Loot")]
    //[Tooltip("The object this enemy can drop when dying")]
    //public GameObject lootPrefab;

    public UnityAction onAttack;
    public UnityAction onDetectedTarget;
    public UnityAction onLostTarget;
    public UnityAction onDamaged;


    List<RendererIndexData> m_BodyRenderers = new List<RendererIndexData>();
    float m_LastTimeDamaged = float.NegativeInfinity;

    public PatrolPath patrolPath { get; set; }
    public GameObject knownDetectedTarget => m_DetectionModule.knownDetectedTarget;
    public bool isTargetInAttackRange => m_DetectionModule.isTargetInAttackRange;
    public bool isSeeingTarget => m_DetectionModule.isSeeingTarget;
    public bool hadKnownTarget => m_DetectionModule.hadKnownTarget;
    public NavMeshAgent m_NavMeshAgent { get; private set; }
    public DetectionModule m_DetectionModule { get; private set; }

    int m_PathDestinationNodeIndex;
    EnemyManager m_EnemyManager;
    CharacterManager m_ActorsManager;
    Health m_Health;
    //Character character; 
    Collider[] m_SelfColliders;
    Game game;
    bool m_WasDamagedThisFrame;
    float m_LastTimeWeaponSwapped = Mathf.NegativeInfinity;
    int m_CurrentWeaponIndex;
    //WeaponController m_CurrentWeapon;
    //WeaponController[] m_Weapons;
    NavigationModule m_NavigationModule;

    void Start()
    {
        m_EnemyManager = FindObjectOfType<EnemyManager>();
        //DebugUtility.HandleErrorIfNullFindObject<EnemyManager, EnemyController>(m_EnemyManager, this);

        m_ActorsManager = FindObjectOfType<CharacterManager>();
        //DebugUtility.HandleErrorIfNullFindObject<ActorsManager, EnemyController>(m_ActorsManager, this);

        m_EnemyManager.RegisterEnemy(this);

        m_Health = GetComponent<Health>();
        //DebugUtility.HandleErrorIfNullGetComponent<Health, EnemyController>(m_Health, this, gameObject);

        //character = GetComponent<Character>();
        //DebugUtility.HandleErrorIfNullGetComponent<Actor, EnemyController>(m_Actor, this, gameObject);

        m_NavMeshAgent = GetComponent<NavMeshAgent>();
        m_SelfColliders = GetComponentsInChildren<Collider>();

        game = FindObjectOfType<Game>();
        //DebugUtility.HandleErrorIfNullFindObject<GameFlowManager, EnemyController>(m_GameFlowManager, this);

        // Subscribe to damage & death actions
        //m_Health.onDie += OnDie;
        //m_Health.onDamaged += OnDamaged;

        // Find and initialize all weapons
        //FindAndInitializeAllWeapons();
        //var weapon = GetCurrentWeapon();
        //weapon.ShowWeapon(true);

        var detectionModules = GetComponentsInChildren<DetectionModule>();
        //DebugUtility.HandleErrorIfNoComponentFound<DetectionModule, EnemyController>(detectionModules.Length, this, gameObject);
        //DebugUtility.HandleWarningIfDuplicateObjects<DetectionModule, EnemyController>(detectionModules.Length, this, gameObject);
        // Initialize detection module
        m_DetectionModule = detectionModules[0];
        m_DetectionModule.onDetectedTarget += OnDetectedTarget;
        m_DetectionModule.onLostTarget += OnLostTarget;
        onAttack += m_DetectionModule.OnAttack;

        var navigationModules = GetComponentsInChildren<NavigationModule>();
        //DebugUtility.HandleWarningIfDuplicateObjects<DetectionModule, EnemyController>(detectionModules.Length, this, gameObject);
        // Override navmesh agent data
        if (navigationModules.Length > 0)
        {
            m_NavigationModule = navigationModules[0];
            m_NavMeshAgent.speed = m_NavigationModule.moveSpeed;
            m_NavMeshAgent.angularSpeed = m_NavigationModule.angularSpeed;
            m_NavMeshAgent.acceleration = m_NavigationModule.acceleration;
        }

        //foreach (var renderer in GetComponentsInChildren<Renderer>(true))
        //{
        //    for (int i = 0; i < renderer.sharedMaterials.Length; i++)
        //    {
        //        if (renderer.sharedMaterials[i] == eyeColorMaterial)
        //        {
        //            m_EyeRendererData = new RendererIndexData(renderer, i);
        //        }

        //        if (renderer.sharedMaterials[i] == bodyMaterial)
        //        {
        //            m_BodyRenderers.Add(new RendererIndexData(renderer, i));
        //        }
        //    }
        //}

        //m_BodyFlashMaterialPropertyBlock = new MaterialPropertyBlock();

        //// Check if we have an eye renderer for this enemy
        //if (m_EyeRendererData.renderer != null)
        //{
        //    m_EyeColorMaterialPropertyBlock = new MaterialPropertyBlock();
        //    m_EyeColorMaterialPropertyBlock.SetColor("_EmissionColor", defaultEyeColor);
        //    m_EyeRendererData.renderer.SetPropertyBlock(m_EyeColorMaterialPropertyBlock, m_EyeRendererData.materialIndex);
        //}
    }

    void Update()
    {
        EnsureIsWithinLevelBounds();

        m_DetectionModule.HandleTargetDetection(this, m_SelfColliders);

        //Color currentColor = onHitBodyGradient.Evaluate((Time.time - m_LastTimeDamaged) / flashOnHitDuration);
        //m_BodyFlashMaterialPropertyBlock.SetColor("_EmissionColor", currentColor);
        //foreach (var data in m_BodyRenderers)
        //{
        //    data.renderer.SetPropertyBlock(m_BodyFlashMaterialPropertyBlock, data.materialIndex);
        //}

        m_WasDamagedThisFrame = false;
    }

    void EnsureIsWithinLevelBounds()
    {
        // at every frame, this tests for conditions to kill the enemy
        if (transform.position.y < Parameters.selfDestructYHeight)
        {
            Destroy(gameObject);
            return;
        }
    }

    void OnLostTarget()
    {
        onLostTarget.Invoke();

        // Set the eye attack color and property block if the eye renderer is set
        //if (m_EyeRendererData.renderer != null)
        //{
        //    m_EyeColorMaterialPropertyBlock.SetColor("_EmissionColor", defaultEyeColor);
        //    m_EyeRendererData.renderer.SetPropertyBlock(m_EyeColorMaterialPropertyBlock, m_EyeRendererData.materialIndex);
        //}
    }

    void OnDetectedTarget()
    {
        onDetectedTarget.Invoke();

        // Set the eye default color and property block if the eye renderer is set
        //if (m_EyeRendererData.renderer != null)
        //{
        //    m_EyeColorMaterialPropertyBlock.SetColor("_EmissionColor", attackEyeColor);
        //    m_EyeRendererData.renderer.SetPropertyBlock(m_EyeColorMaterialPropertyBlock, m_EyeRendererData.materialIndex);
        //}
    }

    public void OrientTowards(Vector3 lookPosition)
    {
        Vector3 lookDirection = Vector3.ProjectOnPlane(lookPosition - transform.position, Vector3.up).normalized;
        if (lookDirection.sqrMagnitude != 0f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * Parameters.orientationSpeed);
        }
    }

    private bool IsPathValid()
    {
        return patrolPath && patrolPath.pathNodes.Count > 0;
    }

    public void ResetPathDestination()
    {
        m_PathDestinationNodeIndex = 0;
    }

    public void SetPathDestinationToClosestNode()
    {
        if (IsPathValid())
        {
            int closestPathNodeIndex = 0;
            for (int i = 0; i < patrolPath.pathNodes.Count; i++)
            {
                float distanceToPathNode = patrolPath.GetDistanceToNode(transform.position, i);
                if (distanceToPathNode < patrolPath.GetDistanceToNode(transform.position, closestPathNodeIndex))
                {
                    closestPathNodeIndex = i;
                }
            }

            m_PathDestinationNodeIndex = closestPathNodeIndex;
        }
        else
        {
            m_PathDestinationNodeIndex = 0;
        }
    }

    public Vector3 GetDestinationOnPath()
    {
        if (IsPathValid())
        {
            return patrolPath.GetPositionOfPathNode(m_PathDestinationNodeIndex);
        }
        else
        {
            return transform.position;
        }
    }

    public void SetNavDestination(Vector3 destination)
    {
        if (m_NavMeshAgent)
        {
            m_NavMeshAgent.SetDestination(destination);
        }
    }

    public void UpdatePathDestination(bool inverseOrder = false)
    {
        if (IsPathValid())
        {
            // Check if reached the path destination
            if ((transform.position - GetDestinationOnPath()).magnitude <= Parameters.pathReachingRadius)
            {
                // increment path destination index
                m_PathDestinationNodeIndex = inverseOrder ? (m_PathDestinationNodeIndex - 1) : (m_PathDestinationNodeIndex + 1);
                if (m_PathDestinationNodeIndex < 0)
                {
                    m_PathDestinationNodeIndex += patrolPath.pathNodes.Count;
                }
                if (m_PathDestinationNodeIndex >= patrolPath.pathNodes.Count)
                {
                    m_PathDestinationNodeIndex -= patrolPath.pathNodes.Count;
                }
            }
        }
    }

    void OnDamaged(float damage, GameObject damageSource)
    {
        // test if the damage source is the player
        if (damageSource && damageSource.GetComponent<Player>())
        {
            // pursue the player
            m_DetectionModule.OnDamaged(damageSource);

            if (onDamaged != null)
            {
                onDamaged.Invoke();
            }
            m_LastTimeDamaged = Time.time;

            //// play the damage tick sound
            //if (damageTick && !m_WasDamagedThisFrame)
            //    //AudioUtility.CreateSFX(damageTick, transform.position, AudioUtility.AudioGroups.DamageTick, 0f);

            m_WasDamagedThisFrame = true;
        }
    }

    void OnDie()
    {
        // spawn a particle system when dying
        //var vfx = Instantiate(deathVFX, deathVFXSpawnPoint.position, Quaternion.identity);
        //Destroy(vfx, 5f);

        // tells the game flow manager to handle the enemy destuction
        m_EnemyManager.UnregisterEnemy(this);

        //// loot an object
        //if (TryDropItem())
        //{
        //    Instantiate(lootPrefab, transform.position, Quaternion.identity);
        //}

        // this will call the OnDestroy function
        Destroy(gameObject, Parameters.deathDuration);
    }

    private void OnDrawGizmosSelected()
    {
        // Path reaching range
        //Gizmos.color = pathReachingRangeColor;
        Gizmos.DrawWireSphere(transform.position, Parameters.pathReachingRadius);

        if (m_DetectionModule != null)
        {
            // Detection range
            //Gizmos.color = detectionRangeColor;
            Gizmos.DrawWireSphere(transform.position, m_DetectionModule.detectionRange);

            // Attack range
            //Gizmos.color = attackRangeColor;
            Gizmos.DrawWireSphere(transform.position, m_DetectionModule.attackRange);
        }
    }

    //public void OrientWeaponsTowards(Vector3 lookPosition)
    //{
    //    for (int i = 0; i < m_Weapons.Length; i++)
    //    {
    //        // orient weapon towards player
    //        Vector3 weaponForward = (lookPosition - m_Weapons[i].weaponRoot.transform.position).normalized;
    //        m_Weapons[i].transform.forward = weaponForward;
    //    }
    //}

    //public bool TryAtack(Vector3 enemyPosition)
    //{
    //    if (game.paused)
    //        return false;

    //    OrientWeaponsTowards(enemyPosition);

    //    if ((m_LastTimeWeaponSwapped + delayAfterWeaponSwap) >= Time.time)
    //        return false;

    //    // Shoot the weapon
    //    bool didFire = GetCurrentWeapon().HandleShootInputs(false, true, false);

    //    if (didFire && onAttack != null)
    //    {
    //        onAttack.Invoke();

    //        if (swapToNextWeapon && m_Weapons.Length > 1)
    //        {
    //            int nextWeaponIndex = (m_CurrentWeaponIndex + 1) % m_Weapons.Length;
    //            SetCurrentWeapon(nextWeaponIndex);
    //        }
    //    }

    //    return didFire;
    //}

    ////public bool TryDropItem()
    ////{
    ////    if (dropRate == 0 || lootPrefab == null)
    ////        return false;
    ////    else if (dropRate == 1)
    ////        return true;
    ////    else
    ////        return (UnityEngine.Random.value <= dropRate);
    ////}

    //void FindAndInitializeAllWeapons()
    //{
    //    // Check if we already found and initialized the weapons
    //    if (m_Weapons == null)
    //    {
    //        m_Weapons = GetComponentsInChildren<WeaponController>();
    //        //DebugUtility.HandleErrorIfNoComponentFound<WeaponController, EnemyController>(m_Weapons.Length, this, gameObject);

    //        for (int i = 0; i < m_Weapons.Length; i++)
    //        {
    //            m_Weapons[i].owner = gameObject;
    //        }
    //    }
    //}

    //public WeaponController GetCurrentWeapon()
    //{
    //    FindAndInitializeAllWeapons();
    //    // Check if no weapon is currently selected
    //    if (m_CurrentWeapon == null)
    //    {
    //        // Set the first weapon of the weapons list as the current weapon
    //        SetCurrentWeapon(0);
    //    }
    //    //DebugUtility.HandleErrorIfNullGetComponent<WeaponController, EnemyController>(m_CurrentWeapon, this, gameObject);

    //    return m_CurrentWeapon;
    //}

    //void SetCurrentWeapon(int index)
    //{
    //    m_CurrentWeaponIndex = index;
    //    m_CurrentWeapon = m_Weapons[m_CurrentWeaponIndex];
    //    if (swapToNextWeapon)
    //    {
    //        m_LastTimeWeaponSwapped = Time.time;
    //    }
    //    else
    //    {
    //        m_LastTimeWeaponSwapped = Mathf.NegativeInfinity;
    //    }
    //}
}
