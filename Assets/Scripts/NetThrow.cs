using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class NetThrow : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject indicator;
    [SerializeField] private GameObject net;
    [SerializeField] private GameObject indicatorHolder;
    [SerializeField] private GameObject netHolder;
    [SerializeField] private Slider netChargeBar;

    private SphereCollider netPickupHB;
    private WaterFloat netWaterFloat;

    [Header("Charge")]
    [SerializeField] private float maxDistance = 20f;
    [SerializeField] private float chargeTime = 2f;
    [SerializeField] private float reelSpeed = 5f;

    public float currentReelSpeed = 0f;

    private enum NetState
    {
        Unequipped,
        Equipped,
        Charging,
        Throwing,
        Thrown,
        Reeling,
        WaitingForRelease
    }

    [SerializeField] private NetState netState = NetState.Unequipped;

    private float currentCharge;

    private bool charging;


    private void Awake()
    {
        net.SetActive(false);
        indicator.SetActive(false);
        netPickupHB = net.GetComponent<SphereCollider>();
        netPickupHB.enabled = false;
        netWaterFloat = net.GetComponent<WaterFloat>();
        netChargeBar.gameObject.SetActive(false);
    }

    private void Update()
    {
        switch (netState)
        {
            case NetState.Charging:
                currentCharge += Time.deltaTime;
                float percent = Mathf.Clamp01(currentCharge / chargeTime);
                indicator.transform.localPosition = Vector3.forward * (percent * maxDistance);
                netChargeBar.value = percent;
                break;

            case NetState.Reeling:
                Vector3 reelPos = new Vector3 (netHolder.transform.position.x, 0, netHolder.transform.position.z);
                net.transform.position = Vector3.MoveTowards(net.transform.position, reelPos, currentReelSpeed * Time.deltaTime);
                break;
        }
    }

    public void EquipNet()
    {
        if (netState == NetState.Unequipped)
        {
            net.SetActive(true);
            indicator.SetActive(false);

            indicator.transform.SetParent(indicatorHolder.transform, false);
            indicator.transform.localPosition = Vector3.zero;

            net.transform.SetParent(netHolder.transform, false);
            net.transform.localPosition = Vector3.zero;
            net.transform.localRotation = Quaternion.Euler(0f, 0f, -90f);

            netState = NetState.Equipped;
        }
        else if (netState == NetState.Equipped)
        {
            net.SetActive(false);
            indicator.SetActive(false);

            netState = NetState.Unequipped;
        }
    }

    public void OnChargeNet(InputAction.CallbackContext context)
    {
        if (context.canceled && netState == NetState.WaitingForRelease)
        {
            netState = NetState.Equipped;
            return;
        }

        if (context.started)
        {
            switch (netState)
            {
                case NetState.Equipped:
                    currentCharge = 0f;
                    net.transform.SetParent(netHolder.transform);
                    indicator.transform.SetParent(indicatorHolder.transform);
                    indicator.SetActive(true);
                    netChargeBar.gameObject.SetActive(true);
                    netState = NetState.Charging;
                    break;

                case NetState.Thrown:
                    currentReelSpeed = reelSpeed;
                    if (netPickupHB.enabled == false) netPickupHB.enabled = true;
                    netState = NetState.Reeling;
                    break;

                case NetState.Reeling:
                    currentReelSpeed = reelSpeed;
                    break;
            }
        }

        if (context.canceled)
        {
            switch (netState)
            {
                case NetState.Charging:
                    Vector3 throwTarget = indicator.transform.position;
                    indicator.transform.SetParent(null);
                    netChargeBar.gameObject.SetActive(false);
                    StartCoroutine(ThrowNet(throwTarget));
                    break;

                case NetState.Reeling:
                    currentReelSpeed = 0f;
                    break;
            }
        }
    }

    private IEnumerator ThrowNet(Vector3 target)
    {
        netState = NetState.Throwing;

        Vector3 startPos = net.transform.position;

        float duration = 1f;
        float arcHeight = 2f;
        float elapsed = 0f;

        net.transform.SetParent(null);
        net.transform.localRotation = Quaternion.identity;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            Vector3 position = Vector3.Lerp(startPos, target, t);
            position.y += arcHeight * 4f * t * (1f - t);

            net.transform.position = position;
            indicator.SetActive(false);
            
            yield return null;
        }
        net.transform.position = target;
        netState = NetState.Thrown;
        netWaterFloat.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Overlapped tag: " + other.tag);

        if (netState == NetState.Reeling && other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Found Player");
            netPickupHB.enabled = false;
            ResetNetToHand();
            netWaterFloat.enabled = false;
            //netState = NetState.WaitingForRelease;
        }
    }

    private void ResetNetToHand()
    {
        net.transform.SetParent(netHolder.transform, false);
        net.transform.localPosition = Vector3.zero;
        net.transform.localRotation = Quaternion.Euler(0f, 0f, -90f);
        netState = NetState.WaitingForRelease;
    }
}