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
        //level = Utilities.GetRandomFromRange(levelRange);
        level = levelsCompleted;

        int numberOfEnemyTypes = Enum.GetNames(typeof(EnemyType)).Length;
        int[] typeNumRange = { 0, numberOfEnemyTypes };
        int enemyTypeNum = Utilities.GetRandomFromRange(typeNumRange);
        enemyType = (EnemyType)Enum.Parse(typeof(EnemyType), Enum.GetNames(typeof(EnemyType))[enemyTypeNum]);

        stats.SetStrength(10);
        stats.SetAgility(10);
        stats.SetVitality(10);

        stats.UpdateStats();
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

    public UnityAction onAttack;
    public UnityAction onDetectedTarget;
    public UnityAction onLostTarget;
    public UnityAction onDamaged;

    public GameObject meshObject;

    public PatrolPath patrolPath { get; set; }
    public GameObject knownDetectedTarget => m_DetectionModule.knownDetectedTarget;
    public Character currentOpponent => m_DetectionModule.detectedEnemy;
    public bool isTargetInAttackRange => m_DetectionModule.isTargetInAttackRange;
    public bool isSeeingTarget => m_DetectionModule.isSeeingTarget;
    public bool hadKnownTarget => m_DetectionModule.hadKnownTarget;
    public NavMeshAgent m_NavMeshAgent { get; private set; }
    public DetectionModule m_DetectionModule { get; private set; }

    int m_PathDestinationNodeIndex;
    CharacterManager characterManager;
    EnemyManager enemyManager;
    Collider[] m_SelfColliders;
    NavigationModule m_NavigationModule;

    const int attackTimeout = 60; // in frames
    int framesUntilNextAttack = 0;

    void Start()
    {
        enemyManager = FindObjectOfType<EnemyManager>();
        //DebugUtility.HandleErrorIfNullFindObject<EnemyManager, EnemyController>(m_EnemyManager, this);

        GenerateStats(1);
        enemyManager.RegisterEnemy(this);

        //character = GetComponent<Character>();
        //DebugUtility.HandleErrorIfNullGetComponent<Actor, EnemyController>(m_Actor, this, gameObject);

        m_NavMeshAgent = GetComponent<NavMeshAgent>();
        m_SelfColliders = GetComponentsInChildren<Collider>();

        characterManager = GameObject.FindObjectOfType<CharacterManager>();

        if (!characterManager.characters.Contains(this))
        {
            characterManager.characters.Add(this);
        }

        var detectionModules = GetComponentsInChildren<DetectionModule>();
 
        // Initialize detection module
        m_DetectionModule = detectionModules[0];
        m_DetectionModule.onDetectedTarget += OnDetectedTarget;
        m_DetectionModule.onLostTarget += OnLostTarget;

        var navigationModules = GetComponentsInChildren<NavigationModule>();

        // Override navmesh agent data
        if (navigationModules.Length > 0)
        {
            m_NavigationModule = navigationModules[0];
            m_NavMeshAgent.speed = m_NavigationModule.moveSpeed;
            m_NavMeshAgent.angularSpeed = m_NavigationModule.angularSpeed;
            m_NavMeshAgent.acceleration = m_NavigationModule.acceleration;
        }

        affiliation = 1; // Enemy Team

        meshObject.GetComponent<Renderer>().material.color = new Color(0, 0, 1);

    }

    void Update()
    {
        EnsureIsWithinLevelBounds();

        m_DetectionModule.HandleTargetDetection(this, m_SelfColliders);

        if (framesUntilNextAttack > 0) framesUntilNextAttack--;
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
        meshObject.GetComponent<Renderer>().material.color = new Color(0, 0, 255);
        onLostTarget.Invoke();
    }

    void OnDetectedTarget()
    {
        meshObject.GetComponent<Renderer>().material.color = new Color(5, 1, 0);
        onDetectedTarget.Invoke();

    }

    public void OnReachedTarget()
    {
        meshObject.GetComponent<Renderer>().material.color = new Color(8, 0, 0);
        TryAttack();
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
        }
    }

    void OnDie()
    {
        // tells the game flow manager to handle the enemy destuction
        enemyManager.UnregisterEnemy(this);

        // this will call the OnDestroy function
        Destroy(gameObject, Parameters.deathDuration);

        // Unregister as an actor
        if (characterManager)
        {
            characterManager.characters.Remove(this);
        }
    }

    public void TryAttack()
    {
        if (framesUntilNextAttack > 0) return;

        int enemyAttack = stats.GetRandomAttack();
        int opponentDefense = currentOpponent.stats.GetRandomDefense();

        // if attack lands
        if (enemyAttack > opponentDefense)
        {
            int enemyDamage = stats.GetRandomAttack();
            currentOpponent.characterHealth.TakeDamage(enemyDamage);
            framesUntilNextAttack = attackTimeout;
        }
    }
}
