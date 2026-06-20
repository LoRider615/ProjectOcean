using UnityEngine;
using UnityEngine.InputSystem;

public class TPCamera : MonoBehaviour
{ 
    [Header("Target")]
    public Transform target;
    public Vector3 targetOffset = new Vector3(0f, 2f, 0f);

    [Header("Rotation")]
    public float sensitivityX = 3f;
    public float sensitivityY = 2f;
    public float minPitch = -30f;
    public float maxPitch = 70f;

    [Header("Distance")]
    public float distance = 5f;
    public float minDistance = 2f;
    public float maxDistance = 8f;
    public float zoomSpeed = 2f;

    [Header("Smoothing")]
    public float positionSmoothTime = 0.05f;

    [Header("Collision")]
    public LayerMask collisionMask;

    float yaw;
    float pitch;

    public float scroll;

    Vector3 currentVelocity;

    private Vector2 lookInput;
    private Rigidbody boatRB;
    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;

        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        if (!target) return;

        HandleInput();
        UpdateCameraPosition();
    }

    void HandleInput()
    {
        yaw += lookInput.x * sensitivityX;
        pitch -= lookInput.y * sensitivityY;

        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        distance -= scroll * zoomSpeed;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);
    }

    void UpdateCameraPosition()
    {
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);

        Vector3 desiredPosition =
            target.position +
            targetOffset -
            (rotation * Vector3.forward * distance);

        // --- Collision check ---
        Vector3 direction = desiredPosition - (target.position + targetOffset);
        float dist = direction.magnitude;

        if (Physics.Raycast(target.position + targetOffset, direction.normalized, out RaycastHit hit, dist, collisionMask))
        {
            desiredPosition = hit.point;
        }

        transform.position = Vector3.SmoothDamp(
            transform.position,
            desiredPosition,
            ref currentVelocity,
            positionSmoothTime
        );

        transform.rotation = rotation;
    }

    public void BoatZoom(InputAction.CallbackContext context)
    {
        scroll = context.ReadValue<float>();
    }
}
