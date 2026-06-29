using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; set; }

    [SerializeField]
    private GameObject interactText, speed0, speed1, speed2, speed3;
    [SerializeField]
    private GameObject speed0Text, speed1Text, speed2Text, speed3Text;


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
        //Debug.Log("Show text fired");
        interactText.SetActive(true);
    }

    public void HideInteractText()
    {
        interactText.SetActive(false);
    }

    public void ShowBoatSpeedometer(int currentSpeedLevel)
    {
        speed0.SetActive(true);
        SetSpeedNum(currentSpeedLevel);
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

    public void SetSpeedNum(int speedNum)
    {
        switch (speedNum)
        {
            case 0:
                speed0Text.SetActive(true);
                speed1Text.SetActive(false);
                speed2Text.SetActive(false);
                speed3Text.SetActive(false);
                break;
            case 1:
                speed0Text.SetActive(false);
                speed1Text.SetActive(true);
                speed2Text.SetActive(false);
                speed3Text.SetActive(false);
                break;
            case 2:
                speed0Text.SetActive(false);
                speed1Text.SetActive(false);
                speed2Text.SetActive(true);
                speed3Text.SetActive(false);
                break;
            case 3:
                speed0Text.SetActive(false);
                speed1Text.SetActive(false);
                speed2Text.SetActive(false);
                speed3Text.SetActive(true);
                break;
        }
    }




}
