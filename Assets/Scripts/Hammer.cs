using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Hammer : MonoBehaviour, IWeapon
{
    [SerializeField] private bool isCharged = true;
    [SerializeField] private float minVelocity = 0.5f;

    [SerializeField] public int dmg = 1;
    [SerializeField] public int cost = 100;

    private Rigidbody rb;
    private bool activated = false;

    private void Awake()
    {
        rb = GetComponentInChildren<Rigidbody>();
    }

    public int getDmg()
    {
        return dmg;
    }

    public bool Buy()
    {
        if (activated) return false;

        if (!CoinManager.Instance.HasEnoughCoins(cost))
        {
            Debug.Log("Not enough coins");
            return false;
        }

        CoinManager.Instance.RemoveCoins(cost);

        activated = true;
        GetComponent<XRGrabInteractable>().enabled = true;

        return true;
    }

    public bool CanHit()
    {
        // if (!isCharged) return false;
        if (rb == null) return false;

        return rb.linearVelocity.magnitude >= minVelocity;
    }

    public void OnHit()
    {
        isCharged = false;
    }

    public void Recharge()
    {
        isCharged = true;
    }
}