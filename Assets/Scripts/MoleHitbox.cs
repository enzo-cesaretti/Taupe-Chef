using UnityEngine;

public class MoleHitbox : MonoBehaviour
{

    private Mole mole;

    private void Start()
    {
        mole = GetComponentInParent<Mole>();
    }


    private void OnTriggerEnter(Collider other)
    {
        var weapon = other.GetComponentInParent<IWeapon>();
        if (weapon == null) return;

        if (mole.State != Mole.MoleState.Up) return;
        if (!weapon.CanHit()) return;

        mole.RegisterHit();

        weapon.OnHit();
    }
}
