using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class ItemPickup : MonoBehaviour
{
    [Tooltip("Frequency at which the item will move up and down")]
    public float verticalBobFrequency = 1f;
    [Tooltip("Distance the item will move up and down")]
    public float bobbingAmount = 1f;
    [Tooltip("Rotation angle per second")]
    public float rotatingSpeed = 360f;

    //[Tooltip("Sound played on pickup")]
    //public AudioClip pickupSFX;
    //[Tooltip("VFX spawned on pickup")]
    //public GameObject pickupVFXPrefab;

    public UnityAction<Player> onPick;
    public Rigidbody pickupRigidbody { get; private set; }

    Collider m_Collider;
    Vector3 m_StartPosition;
    //bool m_HasPlayedFeedback;

    private void Start()
    {
        pickupRigidbody = GetComponent<Rigidbody>();
        //DebugUtility.HandleErrorIfNullGetComponent<Rigidbody, Pickup>(pickupRigidbody, this, gameObject);
        m_Collider = GetComponent<Collider>();
        //DebugUtility.HandleErrorIfNullGetComponent<Collider, Pickup>(m_Collider, this, gameObject);

        // ensure the physics setup is a kinematic rigidbody trigger
        pickupRigidbody.isKinematic = true;
        m_Collider.isTrigger = true;

        // Remember start position for animation
        m_StartPosition = transform.position;
    }

    private void Update()
    {
        //// Handle bobbing
        //float bobbingAnimationPhase = ((Mathf.Sin(Time.time * verticalBobFrequency) * 0.5f) + 0.5f) * bobbingAmount;
        //transform.position = m_StartPosition + Vector3.up * bobbingAnimationPhase;

        // Handle rotating
        transform.Rotate(Vector3.up, rotatingSpeed * Time.deltaTime, Space.Self);
    }

    void OnPicked(Player player)
    {
        Health playerHealth = player.characterHealth;
        if (playerHealth && !playerHealth.isFull)
        {
            playerHealth.GiveHealth(healAmount);

            m_Pickup.PlayPickupFeedback();

            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Player pickingPlayer = other.GetComponent<Player>();

        if (pickingPlayer != null)
        {
            if (onPick != null)
            {
                onPick.Invoke(pickingPlayer);
            }
        }
    }

    //public void PlayPickupFeedback()
    //{
    //    if (m_HasPlayedFeedback)
    //        return;

    //    if (pickupSFX)
    //    {
    //        AudioUtility.CreateSFX(pickupSFX, transform.position, AudioUtility.AudioGroups.Pickup, 0f);
    //    }

    //    if (pickupVFXPrefab)
    //    {
    //        var pickupVFXInstance = Instantiate(pickupVFXPrefab, transform.position, Quaternion.identity);
    //    }

    //    m_HasPlayedFeedback = true;
    //}
}
