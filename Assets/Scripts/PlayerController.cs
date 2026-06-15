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
    private int moveSpeed = 5, jumpForce = 5;

    private Vector2 moveInput;
    private Rigidbody rb;

    private bool canJump = true;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 movement = transform.forward * moveInput.y + transform.right * moveInput.x;
        movement.y = 0f;
        movement.Normalize();
        
        rb.MovePosition(rb.position +  movement * moveSpeed * Time.fixedDeltaTime); 
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void Jump()
    {
        if (Physics.Raycast(transform.position, Vector3.down, 1f, ~10) && canJump)
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

    public void InteractPressed()
    {
        EventBus.Publish(new InteractEvent()
    }
}
