using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private int moveSpeed = 5;

    private Vector2 moveInput;
    private Rigidbody rb;


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
}
