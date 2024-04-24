using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [SerializeField] private float _dashForce;
    [SerializeField] private float _dashUpwardForce;
    [SerializeField] private float _dashDuration;
    [SerializeField] private float _dashCoolDown;
    [SerializeField] private float _dashCoolDownTimer;
    [SerializeField] private float _maxDashYSpeed;
    [SerializeField] private float _dashFov;

    [SerializeField] private bool _useCameraForward = true;
    [SerializeField] private bool _allowAllDirections = true;
    [SerializeField] private bool _disableGravity = true;
    [SerializeField] private bool _resetVelocity = true;

    [SerializeField] private Transform _orientation;
    [SerializeField] private Transform _playerCamera;
    [SerializeField] private PlayerCamera _cam;

    [SerializeField] private KeyCode _dashKey = KeyCode.LeftShift;

    private Rigidbody _rb;
    private PlayerMovement _playerMovement;
    private Vector3 _delayedForceToApply;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(_dashKey))
        {
            Dash();
        }

        if (_dashCoolDownTimer > 0)
        {
            _dashCoolDownTimer -= Time.deltaTime;
        }
    }

    private void Dash()
    {
        if (_dashCoolDownTimer > 0)
            return;
        else
            _dashCoolDownTimer = _dashCoolDown;

        Transform forwartTransform;

        if (_useCameraForward)
            forwartTransform = _playerCamera;
        else
            forwartTransform = _orientation;

        _playerMovement.dashing = true;
        _playerMovement._maxYSpeed = _maxDashYSpeed;



        Invoke(nameof(ResetDash), _dashDuration);

        Invoke(nameof(DelayedDashForce), 0.025f);

        Vector3 direction = GetDirection(forwartTransform);

        Vector3 forceToApply = direction * _dashForce + _orientation.up * _dashUpwardForce;

        _rb.AddForce(forceToApply, ForceMode.Impulse);

        _delayedForceToApply = forceToApply;

        if(_disableGravity)
            _rb.useGravity = true;

        _cam.DoFov(_dashFov);
    }

    private void ResetDash()
    {
        _playerMovement.dashing = false;
        _playerMovement._maxYSpeed = 0;

        if(_disableGravity)
            _rb.useGravity = true;

        _cam.DoFov(85f);
    }

    private void DelayedDashForce()
    {
        if (_resetVelocity)
            _rb.velocity = Vector3.zero;

        _rb.AddForce(_delayedForceToApply, ForceMode.Impulse);
    }

    private Vector3 GetDirection(Transform forwardTransform) 
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3();

        if(_allowAllDirections)
            direction = forwardTransform.forward;

        return direction.normalized;
    }
}