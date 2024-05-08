using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class WeaponStateMachine : StateMachine
{
    [Header("States")] [SerializeField] private State _idle;
    [SerializeField] private State _move;
    [SerializeField] private WeaponShootState _shoot;

    [Space(15)] [SerializeField] private PlayerMovement _playerMovement;

    private CompositeDisposable _disposable = new CompositeDisposable();

    public override void OnEnable()
    {
        base.OnEnable();
        Observable.EveryUpdate().Subscribe(_ =>
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && _shoot.CanShoot)
            {
                ChangeState(_shoot);
            }

            if (Input.GetKey(KeyCode.Mouse0) && _shoot.CanShoot)
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