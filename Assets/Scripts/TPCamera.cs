using UnityEngine;
using UnityEngine.InputSystem;

public class TPCamera : MonoBehaviour
{
    public Transform followLocation;

    public float transformSetHeight = 10f;

    

    private void Update()
    {
        transform.position = new Vector3 (followLocation.position.x, transformSetHeight, followLocation.position.z);
        transform.rotation = followLocation.rotation;
    }
}
