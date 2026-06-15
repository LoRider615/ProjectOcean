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
    public bool drivingBoat = false;
    

    [SerializeField]
    private int walkSpeed = 5, jumpForce = 5, sprintSpeed = 8;
    [SerializeField]
    private GameObject playerCam, boat;
    [SerializeField]
    private Transform TPCampos, FPCampos;

    
    public Vector3 anchorToPos;

    private int moveSpeed;

    private Vector2 moveInput;
    private Rigidbody rb;

    private bool canJump = true;

    public bool nearSteeringWheel = false;


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

    private void Interact()
    {
        if (nearSteeringWheel && !drivingBoat)
        {
            drivingBoat = true;
            transform.position = anchorToPos;
            boat.transform.SetParent(transform);
            playerCam.transform.position = TPCampos.position;
            playerCam.transform.rotation = TPCampos.rotation;
        }
    }


}
