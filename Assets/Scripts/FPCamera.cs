using NUnit.Framework.Internal.Commands;
using UnityEngine;
using UnityEngine.InputSystem;

public class FPCamera : MonoBehaviour
{
    [SerializeField]
    private Transform playerBody;
    [SerializeField]
    private float sensitivity = 0.1f;
    [SerializeField]
    private float bottomClamp = -80f;
    [SerializeField]
    private float topClamp = 80f;

    private Vector2 lookInput;
    private float pitch;

    public PlayerController playerController;

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        float mouseX = lookInput.x * sensitivity;
        float mouseY = lookInput.y * sensitivity;

        if (!playerController.drivingBoat)
        {
            playerBody.Rotate(Vector3.up * mouseX);

            pitch -= mouseY;
            pitch = Mathf.Clamp(pitch, bottomClamp, topClamp);

            transform.localRotation = Quaternion.Euler(pitch, 0, 0);
        }
        else
        {

        }
        
    }

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
