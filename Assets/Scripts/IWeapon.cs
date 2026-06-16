using UnityEngine;

public interface IWeapon
{
    int getDmg();
    bool CanHit();
    void OnHit();
    void Recharge();
}