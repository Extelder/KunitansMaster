using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _sprintSpeed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _dashSpeed;

    [SerializeField] private float _playerHeight;
    [SerializeField] private float _additionalJumps = 2;
    [SerializeField] private float _groundDrag;
    [SerializeField] private float _groundCheckRadius;
    [SerializeField] private float _jumpCoolDown;
    [SerializeField] private float _airMultiplier;
    [SerializeField] private float _maxSlopeAngle;
    [SerializeField] private float _dashSpeedChangeFactor;


    [SerializeField] private Transform _orientation;
    [SerializeField] private Transform _groundCheckPoint;
    [SerializeField] private LayerMask _whatIsGround;

    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode _sprintKey = KeyCode.Space;

    [SerializeField] private MovementState _state;

    private MovementState _lastState;

    private float _horiozntalInput;
    private float _verticallInput;
    private float _jumps = 2;
    private float _movementSpeed;
    private float _desiredMoveSpeed;
    private float _lastDesiredMoveSpeed;
    private float _speedChangeFactor;

    private bool _grounded;
    private bool _readyToJump = true;
    private bool _exitingSlope;
    private bool _keepMoment;


    private Vector3 _movementDirection;
    private Rigidbody _rb;

    private RaycastHit _slopeHit;

    public float _maxYSpeed;

    [SerializeField]
    private enum MovementState
    {
        Walking,
        Sprinting,
        Air,
        Dashing
    }

    public bool dashing;

    public bool Moving { get; private set; }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_groundCheckPoint.position, _groundCheckRadius);
    }

    private void Update()
    {
        if (_movementDirection.normalized.x <= 0 || _movementDirection.normalized.z <= 0)
        {
            Moving = false;
        }

        SpeedControl();
        MyInput();
        StateHandler();

        //_grounded = Physics.Raycast(transform.position, Vector3.down, _playerHeight * 0.5f + 0.2f, _whatIsGround);
        Collider[] others = Physics.OverlapSphere(_groundCheckPoint.position, _groundCheckRadius, _whatIsGround);

        Debug.Log(others.Length);

        if (others.Length > 0)
        {
            _jumps = _additionalJumps;
            _grounded = true;
        }
        else
        {
            _grounded = false;
        }


        if (_state == MovementState.Walking || _state == MovementState.Sprinting)
            _rb.drag = _groundDrag;
        else
            _rb.drag = 0;
    }

    private void MyInput()
    {
        _horiozntalInput = Input.GetAxisRaw("Horizontal");
        _verticallInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(_jumpKey) && _readyToJump && _jumps != 0)
        {
            _jumps--;
            _readyToJump = false;

            Jump();

            Invoke(nameof(ReadyToJump), _jumpCoolDown);
        }
    }

    private void MovePlayer()
    {
        if (_state == MovementState.Dashing)
            return; 

        _movementDirection = _orientation.forward * _verticallInput + _orientation.right * _horiozntalInput;

        if (_grounded)
        {
            _rb.AddForce(_movementDirection.normalized * _movementSpeed * 10f, ForceMode.Force);
            if (_movementDirection.normalized.x > 0 || _movementDirection.normalized.z > 0)
            {
                Moving = true;
            }
        }

        else if (!_grounded)
            _rb.AddForce(_movementDirection.normalized * _movementSpeed * 10f * _airMultiplier, ForceMode.Force);

        if (OnSlope() && !_exitingSlope)
        {
            _rb.AddForce(GetSlopeMoveDirection() * _movementSpeed * 20f, ForceMode.Force);

            if (_rb.velocity.y > 0)
                _rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        _rb.useGravity = !OnSlope();
    }


    private void StateHandler()
    {
        if (_grounded && Input.GetKey(_sprintKey))
        {
            _state = MovementState.Sprinting;
            _desiredMoveSpeed = _sprintSpeed;
        }

        else if (_grounded)
        {
            _state = MovementState.Walking;
            _desiredMoveSpeed = _walkSpeed;
        }

        else
        {
            _state = MovementState.Air;

            if (_desiredMoveSpeed < _sprintSpeed)
                _desiredMoveSpeed = _walkSpeed;
            else
                _desiredMoveSpeed = _sprintSpeed;
        }

        if (dashing)
        {
            _state = MovementState.Dashing;
            _desiredMoveSpeed = _dashSpeed;
            _speedChangeFactor = _dashSpeedChangeFactor;
        }

        bool desiredMovementSpeedHasChanged = _desiredMoveSpeed != _lastDesiredMoveSpeed;
        if (_lastState == MovementState.Dashing)
            _keepMoment = true;

        if (desiredMovementSpeedHasChanged)
        {
            if (_keepMoment)
            {
                StopAllCoroutines();
                StartCoroutine(SmoothlyLerpMoveSpeed());
            }
            else
            {
                StopAllCoroutines();
                _movementSpeed = _desiredMoveSpeed;
            }
        }

        _lastDesiredMoveSpeed = _desiredMoveSpeed;
        _lastState = _state;
    }

    private void SpeedControl()
    {


        if (OnSlope() && !_exitingSlope)
        {
            if (_rb.velocity.magnitude > _movementSpeed)
                _rb.velocity = _rb.velocity.normalized * _movementSpeed;
        }

        else
        {
            Vector3 _flatVelocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);

            if (_flatVelocity.magnitude > _movementSpeed)
            {
                Vector3 _limitedVelocity = _flatVelocity.normalized * _movementSpeed;
                _rb.velocity = new Vector3(_limitedVelocity.x, _rb.velocity.y, _limitedVelocity.z);
            }
        }

        if (_maxYSpeed != 0 && _rb.velocity.y > _maxYSpeed)
            _rb.velocity = new Vector3(_rb.velocity.x, _maxYSpeed, _rb.velocity.z);
    }

    private void Jump()
    {
        _exitingSlope = true;

        _rb.velocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);

        _rb.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
    }

    private void ReadyToJump()
    {
        _readyToJump = true;

        _exitingSlope = false;
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out _slopeHit, _playerHeight * 0.5f + 0.3f))
        {
            float _angle = Vector3.Angle(Vector3.up, _slopeHit.normal);
            return _angle < _maxSlopeAngle && _angle != 0f;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.Project(_movementDirection, _slopeHit.normal).normalized;
    }

    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        // smoothly lerp movementSpeed to desired value
        float time = 0;
        float difference = Mathf.Abs(_desiredMoveSpeed - _movementSpeed);
        float startValue = _movementSpeed;

        float boostFactor = _speedChangeFactor;

        while (time < difference)
        {
            _movementSpeed = Mathf.Lerp(startValue, _desiredMoveSpeed, time / difference);

            time += Time.deltaTime * boostFactor;

            yield return null;
        }

        _movementSpeed = _desiredMoveSpeed;
        _speedChangeFactor = 1f;
        _keepMoment = false;
    }
}
