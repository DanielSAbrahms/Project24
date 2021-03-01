using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]

public class PlayerMovementController : MonoBehaviour
{
    //Player Camera variables
    public float cameraHeight = 13f;
    public float cameraDistance = 7f;
    public Camera playerCamera;
    public GameObject targetIndicatorPrefab;

    //Player Controller variables
    public float speed;
    public bool isSprinting;
    public float sprintSpeedMult;

    public float gravity = 14.0f;
    public float maxVelocityChange = 10.0f;

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
        r = GetComponent<Rigidbody>();
        r.freezeRotation = true;

        r.useGravity = true;

        //Instantiate aim target prefab
        if (targetIndicatorPrefab)
        {
            targetObject = Instantiate(targetIndicatorPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        }

        //Hide the cursor
        Cursor.visible = false;
    }

    void FixedUpdate()
    {
        Vector3 cameraOffset = new Vector3(cameraDistance, cameraHeight, 0);
        Vector3 targetVelocity = new Vector3(Input.GetAxis("Vertical") * (cameraDistance >= 0 ? -1 : 1), 0, Input.GetAxis("Horizontal") * (cameraDistance >= 0 ? 1 : -1));
        if (isSprinting) targetVelocity *= speed * sprintSpeedMult;
        else targetVelocity *= speed;

        // Apply a force that attempts to reach our target velocity
        Vector3 velocity = r.velocity;
        Vector3 velocityChange = (targetVelocity - velocity);
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
        velocityChange.y = 0;

        r.AddForce(velocityChange, ForceMode.VelocityChange);

        // We apply gravity manually for more tuning control
        r.AddForce(new Vector3(0, -gravity * r.mass, 0));

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

    public void SetSprintSpeedMult(float newMult)
    {
        sprintSpeedMult = newMult;
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void StartSprinting()
    {
        isSprinting = true;
    }

    public void StopSprinting()
    {
        isSprinting = false;
    }

    // --------------------- Debug ------------------------------------

    public static void DumpToConsole(object obj)
    {
        var output = JsonUtility.ToJson(obj, true);
        Debug.Log(output);
    }
}
