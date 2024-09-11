using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class PingPongMove : MonoBehaviour
{
    [SerializeField]
    private Transform _moveTargetPoint;
    [SerializeField]
    private float _startSpeed;
    [SerializeField]
    private float _speedIncrease;
    private Vector3 _direction;
    private Vector3 _startPos;
    private Vector3 _targetPoint;
    private float _distance;
    private Rigidbody _rb;

    private void Start()
    {
        _startPos = transform.position;
        _targetPoint = _moveTargetPoint.position;
        _moveTargetPoint.parent = null;

        _direction = (_targetPoint - _startPos).normalized;
        _distance = Vector3.Distance(_startPos, _targetPoint);
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (_speedIncrease > 0.01f)
        {
            if (Vector3.Distance(transform.position, _startPos) <= _distance / 2)
            {
                var val = _speedIncrease * Time.deltaTime;
                _rb.velocity += new Vector3(val * _direction.x, val * _direction.y, val * _direction.z);
            }
            else
            {
                var val = (_speedIncrease - 0.2f) * Time.deltaTime; //чуть поменьше, чтобы скорость не упала раньше, чем дойдет до точки конца
                _rb.velocity -= new Vector3(val * _direction.x, val * _direction.y, val * _direction.z);
            }
        }
        else
        {
            _rb.velocity = new Vector3(_startSpeed * _direction.x, _startSpeed * _direction.y, _startSpeed * _direction.z);
        }

        if (Vector3.Distance(transform.position, _startPos) > _distance)
        {
            transform.position = _targetPoint;
            var k = _startPos;
            _startPos = _targetPoint;
            _targetPoint = k;

            _direction = (_targetPoint - _startPos).normalized;
            _rb.velocity = new Vector3(_startSpeed * _direction.x, _startSpeed * _direction.y, _startSpeed * _direction.z);
        }

        SetVelocity();
    }

    private void SetVelocity()
    {
        var speed = _rb.velocity.magnitude;
        speed = Mathf.Max(speed, _startSpeed);
        _rb.velocity = new Vector3(speed * _direction.x, speed * _direction.y, speed * _direction.z);
    }
}