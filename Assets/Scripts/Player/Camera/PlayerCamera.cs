using System;
using UnityEngine;
using DG.Tweening;
using UniRx;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private float _sensX;
    [SerializeField] private float _sensY;

    [SerializeField] private Transform _orientation;
    [SerializeField] private Camera _camera;

    private float _xRotation;
    private float _yRotation;
    private float _zRotation;
    private float _targetZRotation;
    private Tween _fovTween;

    private CompositeDisposable _disposable = new CompositeDisposable();

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        _disposable.Clear();
        _fovTween.Kill();
    }

    private void LateUpdate()
    {
        float _mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * _sensX;
        float _mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * _sensY;

        _yRotation += _mouseX;

        _xRotation -= _mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(_xRotation, _yRotation, _zRotation);
        _orientation.rotation = Quaternion.Euler(0, _yRotation, 0);
   }


    public void DoZ(float endValue, float duration)
    {
        _disposable.Clear();
        _targetZRotation = endValue;
        Observable.EveryUpdate().Subscribe(_ =>
        {
            _zRotation = Mathf.Lerp(_zRotation, _targetZRotation, duration * Time.deltaTime);
        }).AddTo(_disposable);
    }

    public void DoFov(float endValue)
    {
        _fovTween.Kill();
        _fovTween = _camera.DOFieldOfView(endValue, 0.25f);
    }
}