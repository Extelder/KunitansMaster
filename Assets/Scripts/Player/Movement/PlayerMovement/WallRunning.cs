using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunning : MonoBehaviour
{
    [SerializeField] private float _wallRunForce;
    [SerializeField] private float _maxWallRunTIme;
    [SerializeField] private float _wallCheckDistance;
    [SerializeField] private float _minJumpHeight;

    [SerializeField] private LayerMask _whatIsWall;
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] Transform _orientation;

    private float _wallRunTImer;
    private float _horizontalInput;
    private float _verticalInput;

    private RaycastHit _leftWallhit;
    private RaycastHit _rightWallhit;
    private Rigidbody _rb;
    private PlayerMovement _playerMovement;

    private bool _wallLeft;
    private bool _wallRight;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        CheckForWall();
        StateMachine();
    }

    private void FixedUpdate()
    {
        if (_playerMovement.wallRunning)
            WallRunningMovement();
    }

    private void CheckForWall()
    {
        _wallRight = Physics.Raycast(transform.position, _orientation.right, out _rightWallhit, _wallCheckDistance, _whatIsWall);
        _wallLeft = Physics.Raycast(transform.position, -_orientation.right, out _leftWallhit, _wallCheckDistance, _whatIsWall);
    }

    private bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, _minJumpHeight, _whatIsGround);
    }

    private void StateMachine()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

        if ((_wallLeft || _wallRight) && AboveGround())
        {
            if (!_playerMovement.wallRunning)
                StartWallRun();
        }

        else
        {
            if (_playerMovement.wallRunning)
                StopWallRun();
        }
    }

    private void StartWallRun()
    {
        _playerMovement.wallRunning = true;
    }

    private void WallRunningMovement()
    {
        _rb.useGravity = false;
        _rb.velocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);

        Vector3 wallNormal = _wallRight ? _rightWallhit.normal : _leftWallhit.normal;

        Vector3 wallForward = Vector3.Cross(wallNormal, transform.forward);

        _rb.AddForce(wallForward * _wallRunForce, ForceMode.Force);

        if ((_orientation.forward - wallForward).magnitude > (_orientation.forward - -wallForward).magnitude)
            wallForward = -wallForward;
        if(!(_wallLeft && _horizontalInput > 0) && !(_wallRight && _horizontalInput < 0))
            _rb.AddForce(-wallNormal * 100, ForceMode.Force);

    }

    private void StopWallRun()
    {
        _playerMovement.wallRunning = false;
    }
}
