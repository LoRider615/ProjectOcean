using UnityEngine;

public class Interactable : MonoBehaviour
{
    PlayerController controller;
    public Transform anchorPoint;


    private void OnTriggerEnter(Collider other)
    {
        controller = other.GetComponent<PlayerController>();
        if (controller != null)
        {
            UIManager.instance.ShowInteractText();
            controller.nearSteeringWheel = true;
            if (anchorPoint != null)
            {
                controller.anchorToPos = anchorPoint.position;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (controller != null)
        {
            UIManager.instance.HideInteractText();
            controller.nearSteeringWheel = false;
        }
        anchorPoint = null;
    }
}
