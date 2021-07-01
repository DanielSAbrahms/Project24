﻿using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class DetectionModule : MonoBehaviour
{
    [Tooltip("The point representing the source of target-detection raycasts for the enemy AI")]
    public Transform detectionSourcePoint;
    [Tooltip("The max distance at which the enemy can see targets")]
    public float detectionRange = 5f;
    [Tooltip("Time before an enemy abandons a known target that it can't see anymore")]
    public float knownTargetTimeout = 4f;
    [Tooltip("Optional animator for OnShoot animations")]
    public Animator animator;

    public UnityAction onDetectedTarget;
    public UnityAction onLostTarget;

    public GameObject knownDetectedTarget { get; private set; }
    public Character detectedEnemy { get; private set; }
    public bool isTargetInAttackRange { get; private set; }
    public bool isSeeingTarget { get; private set; }
    public bool hadKnownTarget { get; private set; }

    protected float m_TimeLastSeenTarget = Mathf.NegativeInfinity;

    CharacterManager characterManager;

    const string k_AnimAttackParameter = "Attack";
    const string k_AnimOnDamagedParameter = "OnDamaged";

    protected virtual void Start()
    {
        characterManager = FindObjectOfType<CharacterManager>();
        //DebugUtility.HandleErrorIfNullFindObject<ActorsManager, DetectionModule>(m_ActorsManager, this);
    }

    public virtual void HandleTargetDetection(Character character, Collider[] selfColliders)
    {
        // Handle known target detection timeout
        if (knownDetectedTarget && !isSeeingTarget && (Time.time - m_TimeLastSeenTarget) > knownTargetTimeout)
        {
            knownDetectedTarget = null;
        }

        // Find the closest visible hostile actor
        float sqrDetectionRange = detectionRange * detectionRange;
        isSeeingTarget = false;
        float closestSqrDistance = Mathf.Infinity;
        Player player = characterManager.player;
        float sqrDistance = (player.transform.position - detectionSourcePoint.position).sqrMagnitude;

        if (sqrDistance < sqrDetectionRange && sqrDistance < closestSqrDistance)
        {
            // Check for obstructions
            RaycastHit[] hits = Physics.RaycastAll(detectionSourcePoint.position, (player.aimPoint.position - detectionSourcePoint.position).normalized, detectionRange, -1, QueryTriggerInteraction.Ignore);
            RaycastHit closestValidHit = new RaycastHit();
            closestValidHit.distance = Mathf.Infinity;
            bool foundValidHit = false;
            foreach (var hit in hits)
            {
                if (!selfColliders.Contains(hit.collider) && hit.distance < closestValidHit.distance)
                {
                    closestValidHit = hit;
                    foundValidHit = true;
                }
            }

            if (foundValidHit)
            {
                Character hitActor = closestValidHit.collider.GetComponentInParent<Character>();
                if (hitActor == player)
                {
                    isSeeingTarget = true;
                    closestSqrDistance = sqrDistance;

                    m_TimeLastSeenTarget = Time.time;
                    knownDetectedTarget = player.gameObject;
                    detectedEnemy = player;
                }
            }
        }
        if (knownDetectedTarget)
        {
            float distanceFromOpponent = Vector3.Distance(transform.position, knownDetectedTarget.transform.position);
            isTargetInAttackRange = distanceFromOpponent <= Parameters.MELEE_ATTACK_DISTANCE;
        }

        // Detection events
        if (!hadKnownTarget &&
            knownDetectedTarget != null)
        {
            OnDetect();
        }

        if (hadKnownTarget &&
            knownDetectedTarget == null)
        {
            OnLostTarget();
        }

        // Remember if we already knew a target (for next frame)
        hadKnownTarget = knownDetectedTarget != null;
    }

    public virtual void OnLostTarget()
    {
        if (onLostTarget != null)
        {
            onLostTarget.Invoke();
        }
    }

    public virtual void OnDetect()
    {
        if (onDetectedTarget != null)
        {
            onDetectedTarget.Invoke();
        }
    }

    public virtual void OnDamaged(GameObject damageSource)
    {
        m_TimeLastSeenTarget = Time.time;
        knownDetectedTarget = damageSource;

        if (animator)
        {
            animator.SetTrigger(k_AnimOnDamagedParameter);
        }
    }

    public virtual void OnAttack()
    {
        if (animator)
        {
            animator.SetTrigger(k_AnimAttackParameter);
        }
    }
}
