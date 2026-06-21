using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public GameObject redLight, greenLight;
    public Transform respawnPoint;
    public Transform boatRespawnPoint;
    public string spawnName;

    private BoatController boatController;

    private void Awake()
    {
        boatController = GameObject.Find("Boat Holder").GetComponent<BoatController>();
        greenLight.SetActive(false);
        redLight.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().SetRespawn(this);
            TurnOnLight();
            boatController.SetRespawn(boatRespawnPoint);
        }
    }

    public void TurnOnLight()
    {
        greenLight.SetActive(true);
        redLight.SetActive(false);
    }

    public void TurnOffLight()
    {
        greenLight.SetActive(false);
        redLight.SetActive(true);
    }



}
