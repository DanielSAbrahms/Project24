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
                //enemySource.UpdatePathDestination();
                //enemySource.SetNavDestination(enemySource.GetDestinationOnPath());
                break;
            case AIState.Follow:
                enemySource.SetNavDestination(enemySource.knownDetectedTarget.transform.position);
                enemySource.OrientTowards(enemySource.knownDetectedTarget.transform.position);
                break;
            case AIState.Attack:
                enemySource.OrientTowards(enemySource.knownDetectedTarget.transform.position);
                enemySource.OnReachedTarget();
                break;
        }
    }

    void OnAttack()
    {
        if (aiState == AIState.Patrol)
        {
            aiState = AIState.Follow;
        }
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
