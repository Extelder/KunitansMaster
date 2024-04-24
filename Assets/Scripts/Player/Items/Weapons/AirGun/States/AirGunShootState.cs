using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class AirGunShootState : ItemState
{
    [SerializeField] private float _cdTimeInSeconds;
    [SerializeField] private Rigidbody _playerRigidbody;
    [SerializeField] private float _force;
    [SerializeField] private Transform _camera;

    private CompositeDisposable _disposable = new CompositeDisposable();

    public bool CanShoot { get; private set; } = true;

    public override void Enter()
    {
        CanChanged = false;
        CanShoot = false;
        Animator.Shoot();

        PerformShoot();

        CoolDown.Timer(_cdTimeInSeconds, () => { CanShoot = true; }, _disposable);
    }


    public void PerformShoot()
    {
        _playerRigidbody.AddForce(-_camera.forward * _force, ForceMode.Impulse);
    }

    public void AnimationEndCanChanged()
    {
        CanChanged = true;
    }

    private void OnDisable()
    {
        _disposable.Clear();
    }
}