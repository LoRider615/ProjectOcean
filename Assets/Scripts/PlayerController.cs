using System.Collections;
using Unity.VisualScripting;
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
    public Transform respawnLoc;
    public int maxHealth = 3, currentHealth = 3;


    [SerializeField]
    private int walkSpeed = 5, jumpForce = 5, sprintSpeed = 8;
    [SerializeField]
    private GameObject playerCam, boatCam, boat;
    [SerializeField]
    private PlayerInput playerInput;

    private int moveSpeed;

    private Vector2 moveInput;

    private Rigidbody rb;

    private BoatController boatController;

    private bool canJump = true;

    private float steerInput = 0f;

    private Checkpoint lastCheckpoint;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        moveSpeed = walkSpeed;
        playerInput.SwitchCurrentActionMap("PlayerControls");
        boatController = boat.GetComponent<BoatController>();
        boatCam.SetActive(false);
        currentHealth = maxHealth;
    }

    private void FixedUpdate()
    {

        Vector3 movement = transform.forward * moveInput.y + transform.right * moveInput.x;
        movement.y = 0f;
        movement.Normalize();

        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        
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
            playerCam.SetActive(false);
            boatCam.SetActive(true);
            //boatCam.transform.position = new Vector3(1950f, 1100f, 0f);
            playerInput.SwitchCurrentActionMap("BoatControls");
            UIManager.instance.HideInteractText();
            UIManager.instance.ShowBoatSpeedometer(boatController.speedLevel);
        }
    }

    public void Dismount()
    {
        if (drivingBoat)
        {
            playerInput.SwitchCurrentActionMap("PlayerControls");
            drivingBoat = false;
            boatCam.SetActive(false);
            playerCam.SetActive(true);
            UIManager.instance.HideBoatSpeedometer();
        }
    }


    public void ShiftUp(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            int newSpeedLevel = boatController.speedLevel + 1;
            //Debug.Log("New Speed Level: " + newSpeedLevel);
            switch (newSpeedLevel)
            {
                case 1:
                    boatController.SetSpeedLevel(newSpeedLevel);
                    break;
                case 2:
                    boatController.SetSpeedLevel(newSpeedLevel);
                    break;
                case 3:
                    boatController.SetSpeedLevel(newSpeedLevel);
                    break;
                default:
                    Debug.Log("Speed can't go any higher!");
                    break;
            }
        } 
    }

    public void ShiftDown(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            int newSpeedLevel = boatController.speedLevel - 1;
            //Debug.Log("New Speed Level: " + newSpeedLevel);
            switch (newSpeedLevel)
            {
                case 0:
                    boatController.SetSpeedLevel(newSpeedLevel);
                    break;
                case 1:
                    boatController.SetSpeedLevel(newSpeedLevel);
                    break;
                case 2:
                    boatController.SetSpeedLevel(newSpeedLevel);
                    break;
                default:
                    Debug.Log("Speed can't go any lower!");
                    break;
            }
        }
    }

    public void Steer(InputAction.CallbackContext context)
    {
        boatController.steeringInput = context.ReadValue<float>();
    }

    public void Respawn()
    {
        if (respawnLoc != null)
        {
            rb.linearVelocity = Vector3.zero;
            transform.position = respawnLoc.position;
            transform.rotation = respawnLoc.rotation;
            boatController.Respawn();
        }
        else
        {
            Debug.LogWarning("No respawn location found!");
        }
    }

    public void SetRespawn(Checkpoint newBuoy)
    {
        if (lastCheckpoint == null)
        {
            lastCheckpoint = newBuoy;
            respawnLoc = lastCheckpoint.respawnPoint;
        }
        else
        {
            if (lastCheckpoint.name != newBuoy.name)
            {
                lastCheckpoint.TurnOffLight();
                lastCheckpoint = newBuoy;
                respawnLoc = lastCheckpoint.respawnPoint;
            }
        }
        
        
    }



}
