using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Sharkey, Logan
/// Handles Player movement
/// Edited 6/14/2025
/// </summary>
public class PlayerController : MonoBehaviour
{
    public bool drivingBoat = false; public bool nearSteeringWheel = false;
    public Transform playerSteeringAnchorPost;


    [SerializeField]
    private int walkSpeed = 5, jumpForce = 5, sprintSpeed = 8;
    [SerializeField]
    private GameObject playerCam, boat;
    [SerializeField]
    private Transform TPCampos, FPCampos;

    [Header("Boat Params")]
    [SerializeField]
    private float acceleration, maxBoatSpeed, turnSpeed, drag;
    private float boatSpeed;

    private int moveSpeed;

    private Vector2 moveInput;

    private Rigidbody rb;

    private bool canJump = true;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        moveSpeed = walkSpeed;
    }

    private void FixedUpdate()
    {
        if (!drivingBoat)
        {
            Vector3 movement = transform.forward * moveInput.y + transform.right * moveInput.x;
            movement.y = 0f;
            movement.Normalize();

            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
        else
        {
            boatSpeed += moveInput.y * acceleration * Time.fixedDeltaTime;
            boatSpeed = Mathf.Clamp(boatSpeed, -maxBoatSpeed, maxBoatSpeed);
            boatSpeed = Mathf.MoveTowards(boatSpeed, 0, drag * Time.fixedDeltaTime);

            rb.MovePosition(rb.position + transform.forward * boatSpeed * Time.fixedDeltaTime);
            float turnMultiplier = Mathf.Abs(boatSpeed) / maxBoatSpeed;
            rb.MoveRotation(rb.rotation * Quaternion.Euler(0, moveInput.x * turnSpeed * turnMultiplier * Time.fixedDeltaTime, 0));
        }

        
    }

    public void OnMove(InputAction.CallbackContext context)
    {

        moveInput = context.ReadValue<Vector2>();
    }

    public void Jump()
    {
        if (Physics.Raycast(transform.position, Vector3.down, 1.5f, ~10) && canJump && !drivingBoat)
        {
            canJump = false;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            StartCoroutine(JumpCD());
        }
    }

    public IEnumerator JumpCD()
    {
        yield return new WaitForSeconds(0.5f);
        canJump = true;
    }

    public void Crouch(InputAction.CallbackContext context)
    {
        if (!drivingBoat)
        {
            if (context.started)
            {
                Vector3 scale = transform.localScale;
                scale.y = 0.5f;
                transform.localScale = scale;
            }

            if (context.canceled)
            {
                Vector3 scale = transform.localScale;
                scale.y = 1f;
                transform.localScale = scale;
            }
        }
    }
    public void Sprint(InputAction.CallbackContext context)
    {
        if (!drivingBoat)
        {
            if (context.started)
            {
                moveSpeed = sprintSpeed;
            }

            if (context.canceled)
            {
                moveSpeed = walkSpeed;
            }
        }
    }

    public void Interact()
    {
        if (nearSteeringWheel && !drivingBoat)
        {
            drivingBoat = true;
            transform.position = playerSteeringAnchorPost.position;
            boat.transform.SetParent(transform);
            playerCam.transform.position = TPCampos.position;
            playerCam.transform.rotation = TPCampos.rotation;
            rb.useGravity = false;
        }
        else if (nearSteeringWheel && drivingBoat)
        {
            drivingBoat = false;
            boat.transform.SetParent(null);
            playerCam.transform.position = FPCampos.position;
            playerCam.transform.rotation = FPCampos.rotation;
            rb.useGravity = true;
        }
    }


}
