using UnityEngine;

public class BoatController : MonoBehaviour
{
    private bool playerOnBoard = false;

    private float boatCurrentHeading;

    private Rigidbody rb;

    [SerializeField]
    private PlayerController playerController = null;

    [SerializeField]
    private Collider boatFloor;


    public int speedLevel = 0;

    public float currentSpeed;
    public float targetSpeed;
    public float acceleration = 1.7f;
    public float decceleration = 0.9f;
    public float turnSpeed = 2f;
    public float steeringInput;
    public Transform boatRespawnLoc;


    public float[] SpeedTargets =
    { 
        0,     // speedLevel = 0
        2f,    // speedLevel = 1
        5f,    // speedLevel = 2
        10f
    };


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        targetSpeed = SpeedTargets[speedLevel];
        float speedRate = targetSpeed > currentSpeed ? acceleration : decceleration;

        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, speedRate * Time.deltaTime);

        float turnStrength = currentSpeed / (turnSpeed / 4);

        float turnAmt = turnStrength * steeringInput * Time.deltaTime;

        rb.MoveRotation(rb.rotation * Quaternion.Euler(0, turnAmt, 0));
        rb.MovePosition(rb.position + transform.forward * currentSpeed * Time.fixedDeltaTime);
    }

    public void SetSpeedLevel(int level)
    {
        speedLevel = level;
        if (playerController.drivingBoat)
            UIManager.instance.ShowBoatSpeedometer(level);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerController == null)
            {
                playerController = other.GetComponent<PlayerController>();
            }

            other.transform.parent = transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.parent = null;
        }
    }

    public void SetRespawn(Transform newLoc)
    {
        boatRespawnLoc = newLoc;
    }

    public void Respawn()
    {
        transform.position = boatRespawnLoc.position;
        transform.rotation = boatRespawnLoc.rotation;
        SetSpeedLevel(0);
        rb.linearVelocity = Vector3.zero;
        currentSpeed = 0;
    }
}
