using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; set; }

    [SerializeField]
    private GameObject interactText, speed0, speed1, speed2, speed3;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        interactText.SetActive(false);
        HideBoatSpeedometer();
    }

    public void ShowInteractText()
    {
        Debug.Log("Show text fired");
        interactText.SetActive(true);
    }

    public void HideInteractText()
    {
        interactText.SetActive(false);
    }

    public void ShowBoatSpeedometer(int currentSpeedLevel)
    {
        speed0.SetActive(true);
        switch (currentSpeedLevel)
        {
            case 0:
                speed1.SetActive(false);
                speed2.SetActive(false);
                speed3.SetActive(false);
                break;
            case 1:
                speed1.SetActive(true);
                speed2.SetActive(false);
                speed3.SetActive(false);
                break;
            case 2:
                speed1.SetActive(true);
                speed2.SetActive(true);
                speed3.SetActive(false);
                break;
            case 3:
                speed1.SetActive(true);
                speed2.SetActive(true);
                speed3.SetActive(true);
                break;
        } 
    }

    public void HideBoatSpeedometer() 
    { 
        speed0 .SetActive(false);
        speed1 .SetActive(false);
        speed2 .SetActive(false);
        speed3 .SetActive(false);
    }




}
