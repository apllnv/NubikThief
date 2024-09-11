using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField]
    private bool _isPursue;
    [SerializeField]
    private float _speed;
    private Transform _target;
    private Rigidbody _rb;

    private void Start()
    {
        _target = FindObjectOfType<PlayerMove>().transform;
        _rb = GetComponent<Rigidbody>();
        LookAtTarget();
    }

    private void Update()
    {
        LookAtTarget();
        if (_isPursue)
        {
            Pursue();
        }
    }

    private void LookAtTarget()
    {
        transform.LookAt(_target);
    }

    private void Pursue()
    {
        var direction = (_target.position - transform.position).normalized;
        _rb.velocity = new Vector3(_speed * direction.x, _speed * direction.y, _speed * direction.z);
    }
}