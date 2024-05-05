using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.UIElements;

public class PlayerWallRunning : MonoBehaviour
{
    [SerializeField] private float _zCameraRotateDuration = 0.3f;
    [SerializeField] private float _cameraZRotation = 13f;
    [SerializeField] private BoxCollider _leftWallCheckTrigger;
    [SerializeField] private BoxCollider _rightWallCheckTrigger;
    [SerializeField] private PlayerCamera _playerCamera;
    [SerializeField] private PlayerMovement _movement;

    private Rigidbody _rigidbody;
    public bool OnWall;

    private CompositeDisposable _disposable = new CompositeDisposable();

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _leftWallCheckTrigger.OnTriggerEnterAsObservable().Subscribe(_ =>
        {
            if (_movement.Grounded)
                return;
            if (_.TryGetComponent<WallForRunning>(out WallForRunning wallForRunning))
                OnLeftWallDetected();
        }).AddTo(_disposable);

        _rightWallCheckTrigger.OnTriggerEnterAsObservable().Subscribe(_ =>
            {
                if (_movement.Grounded)
                    return;
                if (_.TryGetComponent<WallForRunning>(out WallForRunning wallForRunning))
                    OnRightWallDetected();
            })
            .AddTo(_disposable);

        _leftWallCheckTrigger.OnTriggerExitAsObservable().Subscribe(_ =>
        {
            if (_.TryGetComponent<WallForRunning>(out WallForRunning wallForRunning))
                OnWallLeaved();
        }).AddTo(_disposable);

        _rightWallCheckTrigger.OnTriggerExitAsObservable().Subscribe(_ =>
            {
                if (_.TryGetComponent<WallForRunning>(out WallForRunning wallForRunning))
                    OnWallLeaved();
            })
            .AddTo(_disposable);
    }

    private void OnDisable()
    {
        _disposable.Clear();
    }


    private void OnRightWallDetected()
    {
        OnWallDetected(_cameraZRotation);
    }

    private void OnLeftWallDetected()
    {
        OnWallDetected(-_cameraZRotation);
    }

    private void OnWallDetected(float cameraZRotation)
    {
        _movement.Grounded = true;
        OnWall = true;
        _rigidbody.useGravity = false;
        _rigidbody.velocity = new Vector3(0, 0, 0);
        _playerCamera.DoZ(cameraZRotation, _zCameraRotateDuration);
    }

    private void OnWallLeaved()
    {
        OnWall = false;
        _movement.ResetJumps();
        _rigidbody.useGravity = true;
        _playerCamera.DoZ(0, _zCameraRotateDuration);
    }
}