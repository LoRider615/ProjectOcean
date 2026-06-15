using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = new();

    [SerializeField]
    private GameObject interactText;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        interactText.SetActive(false);
    }

    public void ShowInteractText()
    {
        interactText.SetActive(true);
    }

    public void HideInteractText()
    {
        interactText.SetActive(false);
    }

}
