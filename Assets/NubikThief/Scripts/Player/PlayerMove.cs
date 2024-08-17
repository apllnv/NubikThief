using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private const float _speedRaising = 100f;
    private const float _shiftFromCenterForCheckDistance = 0.01f;

    [SerializeField]
    private float _speedX;
    [SerializeField]
    private float _speedY;
    [SerializeField]
    private int _direction; //1 or -1
    [Space]
    [SerializeField]
    private float _jumpForcce;
    [SerializeField]
    private ForceMode _jumpForceMode;
    [Space]
    [SerializeField]
    private LayerMask _layerMaskCheck;
    [SerializeField]
    private float _maxDistanceBoxCast;
    [SerializeField]
    private bool _isGroundCheck;
    [SerializeField]
    private bool _isLeftCheck;
    [SerializeField]
    private bool _isRightCheck;

    private Rigidbody _rb;
    private Transform _leftCheckPoint;
    private Transform _rightCheckPoint;
    private Transform _groundCheckPoint;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _direction = 1;

        SetCheckColliders();
    }

    private void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            Jump();
        }

        UpdateGroundCheck();
    }

    private void FixedUpdate()
    {
        UpdateVelocityX();
        //UpdateVelocityY();
    }

    private void UpdateVelocityX()
    {
        var x = _speedX * _direction * _speedRaising * Time.fixedDeltaTime;
        var y = _rb.velocity.y;
        var z = _rb.velocity.z;

        _rb.velocity = new Vector3(x, y, z);
    }

    private void UpdateVelocityY()
    {
        var x = _rb.velocity.y;
        var y = _speedY * _speedRaising * Time.fixedDeltaTime;
        var z = _rb.velocity.z;

        _rb.velocity = new Vector3(x, y, z);
    }

    private void Jump()
    {
        _rb.AddForce(new Vector3(0f, _jumpForcce, 0f), _jumpForceMode);
    }

    private void UpdateGroundCheck()
    {
        UpdateGroundCheck(_leftCheckPoint, ref _isLeftCheck);
        UpdateGroundCheck(_rightCheckPoint, ref _isRightCheck);
        UpdateGroundCheck(_groundCheckPoint, ref _isGroundCheck);
    }

    private void UpdateGroundCheck(Transform _checkPoint, ref bool _isGroundParam)
    {
        var heading = _checkPoint.position - transform.position;
        var distance = heading.magnitude;
        var direction = heading / distance;

        var x = _shiftFromCenterForCheckDistance * direction.x;
        var y = _shiftFromCenterForCheckDistance * direction.y;
        var z = _shiftFromCenterForCheckDistance * direction.z;
        var center = transform.position - new Vector3(x, y, z); //отодвигаю центр пуска квадрата, чтобы он не пролетал стену, когда игрок с ней соприкасается
        var halfExtends = transform.localScale * 0.5f;

        _isGroundParam = Physics.BoxCast(center, halfExtends, direction, transform.rotation, _maxDistanceBoxCast, _layerMaskCheck);
    }

    private void SetCheckColliders()
    {
        var childCount = transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            var child = transform.GetChild(i);
            var checkCollider = child.GetComponent<PlayerCheckCollider>();
            if (checkCollider != null)
            {
                if (checkCollider.GerCheckSide() == CheckSide.Left)
                {
                    _leftCheckPoint = checkCollider.transform;
                }
                else if (checkCollider.GerCheckSide() == CheckSide.Right)
                {
                    _rightCheckPoint = checkCollider.transform;
                }
                else if (checkCollider.GerCheckSide() == CheckSide.Ground)
                {
                    _groundCheckPoint = checkCollider.transform;
                }
            }
        }
    }
}