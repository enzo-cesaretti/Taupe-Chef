using UnityEngine;

public interface IWeapon
{
    bool CanHit();
    void OnHit();
    void Recharge();
}