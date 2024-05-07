using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHitBox : MonoBehaviour, IWeaponVisitor
{
    [SerializeField] private UnitHealth _health;

    public void Visit(WeaponShoot weaponShoot)
    {
        _health.TakeDamage(weaponShoot.Weapon.DamagePerHit);
    }
}