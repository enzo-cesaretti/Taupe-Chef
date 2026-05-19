using UnityEngine;

public class Hammer : MonoBehaviour, IWeapon
{
    [SerializeField] private bool isCharged = true;
    [SerializeField] private float minVelocity = 0.5f;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponentInChildren<Rigidbody>();
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