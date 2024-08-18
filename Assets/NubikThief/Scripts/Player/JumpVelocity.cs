using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpVelocity : MonoBehaviour
{
    private Rigidbody _rb;
    public float _endJumpTime;
    public float _jumpAmount;
    [SerializeField]
    private float _jumpTime;
    [SerializeField]
    private bool _isJumping;
    [Space]
    [SerializeField]
    private float _gravityScale;
    [SerializeField]
    private float _slideScale;
    [SerializeField]
    private int _gravityDirection; //1 or -1 не прыгает когда находится на потолке при отрицательной гравитации
    [Space]
    [SerializeField]
    private float _velocitysSmoothTime;

    private Vector3 _velocity;
    private PlayerMove playerMove;


    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        playerMove = GetComponent<PlayerMove>();

        _gravityDirection = 1;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        IsJumping();
        Gravity();

        _rb.velocity = _velocity;
    }

    private void Gravity()
    {
        if (!_isJumping)
        {
            if (playerMove.IsTouchWall())
            {
                ChangeVelocity(_slideScale);
            }
            else
            {
                ChangeVelocity(_gravityScale);
            }
        }
    }

    private void ChangeVelocity(float velocity)
    {
        var startVelocity = _rb.velocity;
        var endVelocity = new Vector3(_rb.velocity.x, velocity * _gravityDirection, _rb.velocity.z);
        Vector3.SmoothDamp(startVelocity, endVelocity, ref _velocity, _velocitysSmoothTime);
    }


    private void IsJumping()
    {
        if (_isJumping)
        {
            ChangeVelocity(_jumpAmount);
            _jumpTime += Time.deltaTime;
        }
        if (_jumpTime > _endJumpTime)
        {
            _isJumping = false;
        }
    }

    private void Jump()
    {
        if (playerMove.IsTouchWall() && !playerMove.IsTouchBottom())
        {
            playerMove.ChangeDirection();
            _isJumping = true;
            _jumpTime = 0;
        }

        if ((playerMove.IsTouchBottom() || (playerMove.IsTouchTop() && _gravityDirection == -1)))
        {
            _isJumping = true;
            _jumpTime = 0;
        }
    }
}