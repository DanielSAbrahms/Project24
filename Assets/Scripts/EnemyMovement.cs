using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMovement : MonoBehaviour
{
    public enum AIState
    {
        Patrol,
        Follow,
        Attack,
    }

    //public Animator animator;
    [Tooltip("Fraction of the enemy's attack range at which it will stop moving towards target while attacking")]
    [Range(0f, 1f)]
    public float attackStopDistanceRatio = 0.5f;
    //[Tooltip("The random hit damage effects")]
    //public ParticleSystem[] randomHitSparks;
    //public ParticleSystem[] onDetectVFX;
    // public AudioClip onDetectSFX;

    //[Header("Sound")]
    //public AudioClip MovementSound;
    //public MinMaxFloat PitchDistortionMovementSpeed;

    public AIState aiState { get; private set; }
    Enemy enemySource;
    //AudioSource m_AudioSource;

    const string k_AnimMoveSpeedParameter = "MoveSpeed";
    const string k_AnimAttackParameter = "Attack";
    const string k_AnimAlertedParameter = "Alerted";
    const string k_AnimOnDamagedParameter = "OnDamaged";

    void Start()
    {
        enemySource = GetComponent<Enemy>();
        //DebugUtility.HandleErrorIfNullGetComponent<Enemy, EnemyMobile>(m_EnemyController, this, gameObject);

        enemySource.onAttack += OnAttack;
        enemySource.onDetectedTarget += OnDetectedTarget;
        enemySource.onLostTarget += OnLostTarget;
        enemySource.SetPathDestinationToClosestNode();
        enemySource.onDamaged += OnDamaged;

        // Start patrolling
        aiState = AIState.Patrol;

    }

    void Update()
    {
        UpdateAIStateTransitions();
        UpdateCurrentAIState();

        float moveSpeed = enemySource.m_NavMeshAgent.velocity.magnitude;

    }

    void UpdateAIStateTransitions()
    {
        // Handle transitions 
        switch (aiState)
        {
            case AIState.Follow:
                // Transition to attack when there is a line of sight to the target
                if (enemySource.isSeeingTarget && enemySource.isTargetInAttackRange)
                {
                    aiState = AIState.Attack;
                    enemySource.SetNavDestination(transform.position);
                }
                break;
            case AIState.Attack:
                // Transition to follow when no longer a target in attack range
                if (!enemySource.isTargetInAttackRange)
                {
                    aiState = AIState.Follow;
                }
                break;
        }
    }

    void UpdateCurrentAIState()
    {
        // Handle logic 
        switch (aiState)
        {
            case AIState.Patrol:
                enemySource.UpdatePathDestination();
                enemySource.SetNavDestination(enemySource.GetDestinationOnPath());
                break;
            case AIState.Follow:
                enemySource.SetNavDestination(enemySource.knownDetectedTarget.transform.position);
                enemySource.OrientTowards(enemySource.knownDetectedTarget.transform.position);
                //m_EnemyController.OrientWeaponsTowards(m_EnemyController.knownDetectedTarget.transform.position);
                break;
            case AIState.Attack:
                if (Vector3.Distance(enemySource.knownDetectedTarget.transform.position, enemySource.m_DetectionModule.detectionSourcePoint.position)
                    >= (attackStopDistanceRatio * enemySource.m_DetectionModule.attackRange))
                {
                    enemySource.SetNavDestination(enemySource.knownDetectedTarget.transform.position);
                }
                else
                {
                    enemySource.SetNavDestination(transform.position);
                }
                enemySource.OrientTowards(enemySource.knownDetectedTarget.transform.position);
                //m_EnemyController.TryAtack(m_EnemyController.knownDetectedTarget.transform.position);
                break;
        }
    }

    void OnAttack()
    {

    }

    void OnDetectedTarget()
    {
        if (aiState == AIState.Patrol)
        {
            aiState = AIState.Follow;
        }

    }

    void OnLostTarget()
    {
        if (aiState == AIState.Follow || aiState == AIState.Attack)
        {
            aiState = AIState.Patrol;
        }
    }

    void OnDamaged()
    {

    }
}
