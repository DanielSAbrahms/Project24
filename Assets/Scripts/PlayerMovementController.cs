using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]

public class PlayerMovementController : MonoBehaviour
{
    //Player Camera variables
    public Camera playerCamera;
    public GameObject targetIndicatorPrefab;
    public CharacterManager cm;

    //Player Controller variables
    public bool isSprinting;

    //Private variables
    Rigidbody r;
    GameObject targetObject;

    //Mouse cursor Camera offset effect
    Vector2 playerPosOnScreen;
    Vector2 cursorPosition;
    Vector2 offsetVector;

    //Plane that represents imaginary floor that will be used to calculate Aim target position
    Plane surfacePlane = new Plane();


    void Awake()
    {
        cm = FindObjectOfType<CharacterManager>();
        r = GetComponent<Rigidbody>();
        r.freezeRotation = true;
        r.useGravity = true;

        //Instantiate aim target prefab
        if (targetIndicatorPrefab)
        {
            targetObject = Instantiate(targetIndicatorPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        }
    }

    void FixedUpdate()
    {
        // Directional Input
        Vector3 cameraOffset = new Vector3(Parameters.CAMERA_DISTANCE, Parameters.CAMERA_HEIGHT, 0);
        Vector3 targetVelocity = new Vector3(Input.GetAxis("Vertical") * (Parameters.CAMERA_DISTANCE >= 0 ? -1 : 1), 0, Input.GetAxis("Horizontal") * (Parameters.CAMERA_DISTANCE >= 0 ? 1 : -1));
        targetVelocity *= Parameters.PLAYER_WALK_SPEED;

        // Sprinting
        if (isSprinting) targetVelocity *= Parameters.SPRINT_SPEED_MULTIPLIER;

        // Apply a force that attempts to reach our target velocity
        Vector3 velocity = r.velocity;
        Vector3 velocityChange = (targetVelocity - velocity);
        velocityChange.x = Mathf.Clamp(velocityChange.x, -Parameters.MAX_VELOCITY_CHANGE, Parameters.MAX_VELOCITY_CHANGE);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -Parameters.MAX_VELOCITY_CHANGE, Parameters.MAX_VELOCITY_CHANGE);
        velocityChange.y = 0;
        r.AddForce(velocityChange, ForceMode.VelocityChange);

        // We apply gravity manually for more tuning control
        r.AddForce(new Vector3(0, -Parameters.GRAVITY * r.mass, 0));

        //Mouse cursor offset effect
        playerPosOnScreen = playerCamera.WorldToViewportPoint(transform.position);
        cursorPosition = playerCamera.ScreenToViewportPoint(Input.mousePosition);
        offsetVector = cursorPosition - playerPosOnScreen;

        //Camera follow
        playerCamera.transform.position = Vector3.Lerp(playerCamera.transform.position, transform.position + cameraOffset, Time.deltaTime * 7.4f);
        playerCamera.transform.LookAt(transform.position + new Vector3(-offsetVector.y * 2, 0, offsetVector.x * 2));

        //Aim target position and rotation
        targetObject.transform.position = GetAimTargetPos();
        targetObject.transform.LookAt(new Vector3(transform.position.x, targetObject.transform.position.y, transform.position.z));

        //Player rotation
        float rotationOffsetX = targetVelocity.x;
        float rotationOffsetY = targetVelocity.y;
        float rotationOffsetZ = targetVelocity.z;
        transform.LookAt(new Vector3(transform.position.x + rotationOffsetX, transform.position.y + rotationOffsetY, transform.position.z + rotationOffsetZ));
    }

    Vector3 GetAimTargetPos()
    {
        //Update surface plane
        surfacePlane.SetNormalAndPosition(Vector3.up, transform.position);

        //Create a ray from the Mouse click position
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

        //Initialise the enter variable
        float enter = 0.0f;

        if (surfacePlane.Raycast(ray, out enter))
        {
            //Get the point that is clicked
            Vector3 hitPoint = ray.GetPoint(enter);

            //Move your cube GameObject to the point where you clicked
            return hitPoint;
        }

        //No raycast hit, hide the aim target by moving it far away
        return new Vector3(-5000, -5000, -5000);
    }

    public Enemy GetClosestEnemyToCursor()
    {
        Enemy temp = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = GetAimTargetPos();
        foreach (Enemy c in cm.enemies)
        {
            float dist = Vector3.Distance(c.gameObject.transform.position, currentPos);
            if (dist < minDist)
            {
                temp = c;
                minDist = dist;
            }
        }
        if (minDist < Parameters.CURSOR_MIN_DISTANCE_TO_OBJECT) return temp;
        else return null;
    }

    public void StartSprinting() { isSprinting = true; }

    public void StopSprinting() { isSprinting = false; }
}
