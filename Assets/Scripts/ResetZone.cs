using UnityEngine;

public class ResetZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var weapon = other.GetComponentInParent<IWeapon>();
        if (weapon == null) return;

        weapon.Recharge();
    }
}
