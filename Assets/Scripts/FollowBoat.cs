using UnityEngine;

public class FollowBoat : MonoBehaviour
{
    public Transform boat;

    // Update is called once per frame
    void Update()
    {
        transform.position = boat.position;
    }
}
