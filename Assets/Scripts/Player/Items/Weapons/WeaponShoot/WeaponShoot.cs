using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WeaponShoot : RaycastBehaviour
{
    [SerializeField] private DefaultWeaponShootState _weaponShootState;

    [field: SerializeField] public RaycastWeaponItem Weapon { get; private set; }

    public Vector3 CurrentShootOffset;

    public event Action ShootPerformed;

    private void OnEnable()
    {
        _weaponShootState.ShootPerformed += OnShootPerformed;
    }

    private void OnDisable()
    {
        _weaponShootState.ShootPerformed -= OnShootPerformed;
    }

    public virtual void OnShootPerformed()
    {
        ShootPerformed?.Invoke();
    }

    public void Accept(IWeaponVisitor visitor)
    {
        visitor.Visit(this);
    }
}