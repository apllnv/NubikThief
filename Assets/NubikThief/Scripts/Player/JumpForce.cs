using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JumpForce : MonoBehaviour
{
    [SerializeField]
    private float _jumpHeight;

    [SerializeField]
    private float _slideForce;
    [SerializeField]
    private float _fallForce;
    [SerializeField]
    private float _maxFallSpeed;
    [SerializeField]
    private float _maxSlideSpeed;
    [SerializeField]
    private float _kayoteTime;
    private Rigidbody _rb;
    private PlayerMove _playerMove;

    private bool _isSlide;
    private bool _isFall;

    private float _lastTouchGroundTime;

    private float _jumpForce;

    private int _gravityDirection;

    private void OnDisable()
    {
        EventBus.OnChangeGravityDirection += ChangeGravityDirection;
    }

    private void OnEnable()
    {
        EventBus.OnChangeGravityDirection += ChangeGravityDirection;
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _playerMove = GetComponent<PlayerMove>();
        _jumpForce = Mathf.Sqrt(_jumpHeight * -2 * (Physics.gravity.y));
        _gravityDirection = -1;
    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            Jump();
        }

        LimitFallVelocity();
        LimitSlideVelocity();
        CheckLastTouchGroundTime();
    }

    private void FixedUpdate()
    {
        Slide();
        Fall();
    }

    private void Jump()
    {
        if (_playerMove.IsTouchBottom() || ((Time.time - _lastTouchGroundTime <= _kayoteTime) && !_playerMove.IsTouchWall()))
        {
            _rb.velocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);
            _rb.AddForce(new Vector3(0, _jumpForce, 0), ForceMode.Impulse);
        }
        else if (_playerMove.IsTouchWall())
        {
            _rb.velocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);
            _rb.AddForce(new Vector3(0, _jumpForce, 0), ForceMode.Impulse);

            _playerMove.ChangeDirection();
        }
    }

    private void CheckLastTouchGroundTime()
    {
        if (_playerMove.IsTouchBottom())
        {
            _lastTouchGroundTime = Time.time;
        }
    }

    private void Slide()
    {
        if (_playerMove.IsTouchWall() && _rb.velocity.y < 0f)
        {
            if (_rb.velocity.y > _maxSlideSpeed)
            {
                _rb.AddForce(new Vector3(0, _slideForce, 0), ForceMode.Force);
            }

            _isSlide = true;
        }
        else
        {
            _isSlide = false;
        }
    }

    private void Fall()
    {
        if (!_playerMove.IsTouchBottom() && !_playerMove.IsTouchWall() && _rb.velocity.y < 0f)
        {
            _rb.AddForce(new Vector3(0, _fallForce, 0), ForceMode.Force);
            _isFall = true;
        }
        else
        {
            _isFall = false;
        }
    }

    private void LimitSlideVelocity()
    {
        if (_isSlide)
        {
            float speed = Mathf.Max(_maxSlideSpeed, _rb.velocity.y);
            _rb.velocity = new Vector3(_rb.velocity.x, speed, _rb.velocity.z);
        }
    }

    private void LimitFallVelocity()
    {
        if (_isFall)
        {
            float speed = Mathf.Max(_maxFallSpeed, _rb.velocity.y);
            _rb.velocity = new Vector3(_rb.velocity.x, speed, _rb.velocity.z);
        }
    }

    private void ChangeGravityDirection()
    {
        _gravityDirection *= -1;
    }


}
