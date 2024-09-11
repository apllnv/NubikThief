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
    private bool _isJumping;
    public bool _isSlide;
    public bool _isFall;
    private float _lastTouchGroundTime;
    private float _jumpForce;
    private int _gravityDirection;


    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _playerMove = GetComponent<PlayerMove>();
        _jumpForce = Mathf.Sqrt(_jumpHeight * -2 * (Physics.gravity.y));
    }

    void Update()
    {
        SetGravityDirection();

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
        bool normalGravityCondition = _playerMove.IsTouchGround() || ((Time.time - _lastTouchGroundTime <= _kayoteTime)
        && !_playerMove.IsTouchWall());

        bool invertedGravityCondition = _playerMove.IsTouchGround() || ((Time.time - _lastTouchGroundTime <= _kayoteTime)
        && !_playerMove.IsTouchWall());

        if (normalGravityCondition || invertedGravityCondition)
        {
            _rb.velocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);
            _rb.AddForce(new Vector3(0, _jumpForce * _playerMove.GravityDirection, 0), ForceMode.Impulse);
            _isJumping = true;

            EventBus.OnAnimatorSetBool?.Invoke(PlayerAnimatorParamentsNames.JUMP, _isJumping);
        }
        else if (_playerMove.IsTouchWall())
        {
            _rb.velocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);
            _rb.AddForce(new Vector3(0, _jumpForce * _playerMove.GravityDirection, 0), ForceMode.Impulse);
            _isJumping = true;

            EventBus.OnPlayerChangeDirection?.Invoke();
            EventBus.OnAnimatorSetBool?.Invoke(PlayerAnimatorParamentsNames.JUMP, _isJumping);
        }
    }

    private void CheckLastTouchGroundTime()
    {
        bool isTouchGround = _playerMove.IsTouchGround();
        if (isTouchGround)
        {
            _lastTouchGroundTime = Time.time;
        }

        EventBus.OnAnimatorSetBool?.Invoke(PlayerAnimatorParamentsNames.RUN, isTouchGround);
    }

    private void Slide()
    {
        if (_playerMove.IsTouchWall() && _rb.velocity.y * _gravityDirection < 0f)
        {
            if (_rb.velocity.y > _maxSlideSpeed)
            {
                _rb.AddForce(new Vector3(0, _slideForce * _gravityDirection, 0), ForceMode.Force);
            }

            _isSlide = true;
            _isJumping = false;

            EventBus.OnAnimatorSetBool?.Invoke(PlayerAnimatorParamentsNames.SLIDE, _isSlide);
            EventBus.OnAnimatorSetBool?.Invoke(PlayerAnimatorParamentsNames.JUMP, _isJumping);
        }
        else
        {
            _isSlide = false;
            EventBus.OnAnimatorSetBool?.Invoke(PlayerAnimatorParamentsNames.SLIDE, _isSlide);
        }
    }

    private void Fall()
    {
        if (!_playerMove.IsTouchGround() && !_playerMove.IsTouchWall() && _rb.velocity.y * _gravityDirection < 0f)
        {
            _rb.AddForce(new Vector3(0, _fallForce * _gravityDirection, 0), ForceMode.Force);
            _isFall = true;
            _isJumping = false;

            EventBus.OnAnimatorSetBool?.Invoke(PlayerAnimatorParamentsNames.FALL, _isFall);
            EventBus.OnAnimatorSetBool?.Invoke(PlayerAnimatorParamentsNames.JUMP, _isJumping);
        }
        else
        {
            _isFall = false;
            EventBus.OnAnimatorSetBool?.Invoke(PlayerAnimatorParamentsNames.FALL, _isFall);
        }
    }

    private void LimitSlideVelocity()
    {
        if (_isSlide)
        {
            float speed = Mathf.Max(_maxSlideSpeed, _rb.velocity.y * _gravityDirection);
            _rb.velocity = new Vector3(_rb.velocity.x, speed * _gravityDirection, _rb.velocity.z);
        }
    }

    private void LimitFallVelocity()
    {
        if (_isFall)
        {
            float speed = Mathf.Max(_maxFallSpeed, _rb.velocity.y * _gravityDirection);
            _rb.velocity = new Vector3(_rb.velocity.x, speed * _gravityDirection, _rb.velocity.z);
        }
    }

    private void SetGravityDirection()
    {
        _gravityDirection = _playerMove.GravityDirection;
    }
}