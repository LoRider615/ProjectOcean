using UnityEngine;

public class ItemBobbing : MonoBehaviour
{
    [SerializeField] private Transform water;


    [SerializeField] private float floatOffset = 0.1f;
    [SerializeField] private float bobAmplitude = 0.15f;
    [SerializeField] private float bobSpeed = 0.8f;
    [SerializeField] private float rockAmount = 4f;
    [SerializeField] private float rockSpeed = 0.5f;

    private float phase;

    private void Awake()
    {
        water = GameObject.Find("Lake").transform;
    }

    private void Start()
    {
        // Gives each object a different motion
        phase = transform.position.x * 0.3f + transform.position.z * 0.2f;
    }

    private void Update()
    {
        float t = Time.time;

        // Two sine waves makes it feel less mechanical
        float bob = Mathf.Sin(t * bobSpeed + transform.position.x * 0.15f + phase) * bobAmplitude + 
                    Mathf.Sin(t * 1.4f + transform.position.z * 0.1f + phase) * bobAmplitude * 0.4f;

        transform.position = new Vector3(
            transform.position.x,
            water.position.y + floatOffset + bob,
            transform.position.z);

        float pitch = Mathf.Sin(t * rockSpeed + phase) * rockAmount;
        float roll = Mathf.Cos(t * rockSpeed * 1.2f + phase) * rockAmount;

        transform.rotation = Quaternion.Euler(pitch, transform.eulerAngles.y, roll);
    }
}
