using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class AirGunStateMachine : StateMachine
{
    [Header("States")] [SerializeField] private State _idle;
    [SerializeField] private State _move;
    [SerializeField] private AirGunShootState _shoot;

    [Space(15)] [SerializeField] private PlayerMovement _playerMovement;

    private CompositeDisposable _disposable = new CompositeDisposable();

    public override void Start()
    {
        base.Start();
        Observable.EveryUpdate().Subscribe(_ =>
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && _shoot.CanShoot)
            {
                ChangeState(_shoot);
            }

            if (_playerMovement.Moving)
            {
                ChangeState(_move);
                Debug.Log("Moving");
                return;
            }

            ChangeState(_idle);
        }).AddTo(_disposable);
    }

    private void OnDisable()
    {
        _disposable.Clear();
    }
}