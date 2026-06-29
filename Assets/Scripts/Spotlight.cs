using UnityEngine;
using UnityEngine.InputSystem;

public class Spotlight : MonoBehaviour
{
    [SerializeField]
    private float sensitivity = 0.1f;
    [SerializeField]
    private float maxHorizontal = 90f;
    [SerializeField]
    private float maxVertical = 40f;

    private Vector2 mouseInput;

    private float yaw, pitch;

    private bool lightOn = false;

    [SerializeField]
    private GameObject spotLight;

    [SerializeField]
    private Transform lightPos;

    [SerializeField]
    private Transform boatTransform;

    private void Awake()
    {
        Vector3 angles = transform.localEulerAngles;
        yaw = angles.y;
        pitch = angles.x;

        if (yaw > 180f) yaw -= 360f;
        if (pitch > 180f) pitch -= 360f;

        lightOn = false;
        spotLight.SetActive(false);

    }

    private void Update()
    {
        yaw += mouseInput.x * sensitivity;
        pitch -= mouseInput.y * sensitivity;

        yaw = Mathf.Clamp(yaw, -maxHorizontal, maxHorizontal);
        pitch = Mathf.Clamp(pitch, -maxVertical, maxVertical);

        float boatYaw = boatTransform.eulerAngles.y;

        transform.rotation = Quaternion.Euler(pitch, yaw + boatYaw, 0f);
        transform.position = lightPos.position;
    }

    public void MouseLook(InputAction.CallbackContext context)
    {
        mouseInput = context.ReadValue<Vector2>();
    }

    public void ToggleLight()
    {

        Debug.Log("Light Toggled");


        if (lightOn)
            spotLight.SetActive(false);
            
        else
            spotLight.SetActive(true);
        
        lightOn = !lightOn;
    }

}
