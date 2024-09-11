using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private const float _speedRaising = 100f;
    private const float _shiftFromCenterForCheckDistance = 0.01f;

    [SerializeField]
    private float _speedX;
    [SerializeField]
    private int _direction; //1 or -1
    [Space]
    [SerializeField]
    private LayerMask _layerMaskCheck;
    [SerializeField]
    private float _maxDistanceBoxCast;
    [SerializeField]
    private bool _isBottomCheck;
    [SerializeField]
    private bool _isLeftCheck;
    [SerializeField]
    private bool _isRightCheck;
    [SerializeField]
    private bool _isTopCheck;
    private Rigidbody _rb;
    private Transform _leftCheckPoint;
    private Transform _rightCheckPoint;
    private Transform _bottomCheckPoint;
    private Transform _topCheckPoint;

    public int GravityDirection { get; private set; }

    private void OnEnable()
    {
        EventBus.OnChangeGravityDirection += ChangeGravityDirection;
        EventBus.OnPlayerChangeDirection += ChangeDirection;
    }

    private void OnDisable()
    {
        EventBus.OnChangeGravityDirection -= ChangeGravityDirection;
        EventBus.OnPlayerChangeDirection -= ChangeDirection;
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _direction = 1;
        GravityDirection = 1;

        SetCheckPoints();
    }

    private void Update()
    {
        UpdateSidesCheck();
    }

    private void FixedUpdate()
    {
        UpdateVelocityX();
    }

    private void UpdateVelocityX()
    {
        var x = _speedX * _direction * _speedRaising * Time.fixedDeltaTime;
        var y = _rb.velocity.y;
        var z = _rb.velocity.z;

        _rb.velocity = new Vector3(x, y, z);
    }

    private void UpdateSidesCheck()
    {
        UpdateSidesCheck(_leftCheckPoint, ref _isLeftCheck);
        UpdateSidesCheck(_rightCheckPoint, ref _isRightCheck);
        UpdateSidesCheck(_bottomCheckPoint, ref _isBottomCheck);
        UpdateSidesCheck(_topCheckPoint, ref _isTopCheck);
    }

    private void UpdateSidesCheck(Transform _checkPoint, ref bool _isGroundParam)
    {
        var heading = _checkPoint.position - transform.position;
        var distance = heading.magnitude;
        var direction = heading / distance;

        var x = _shiftFromCenterForCheckDistance * direction.x;
        var y = _shiftFromCenterForCheckDistance * direction.y;
        var z = _shiftFromCenterForCheckDistance * direction.z;
        var center = transform.position - new Vector3(x, y, z); //отодвигаю центр пуска квадрата, чтобы он не пролетал стену, когда игрок с ней соприкасается
        var halfExtends = transform.localScale * 0.45f;

        _isGroundParam = Physics.BoxCast(center, halfExtends, direction, transform.rotation, _maxDistanceBoxCast, _layerMaskCheck);
    }

    private void SetCheckPoints()
    {
        var childCount = transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            var child = transform.GetChild(i);
            var checkPoint = child.GetComponent<CheckPoint>();
            if (checkPoint != null)
            {
                if (checkPoint.GetCheckSide() == CheckSide.Left)
                {
                    _leftCheckPoint = checkPoint.transform;
                }
                else if (checkPoint.GetCheckSide() == CheckSide.Right)
                {
                    _rightCheckPoint = checkPoint.transform;
                }
                else if (checkPoint.GetCheckSide() == CheckSide.Bottom)
                {
                    _bottomCheckPoint = checkPoint.transform;
                }
                else if (checkPoint.GetCheckSide() == CheckSide.Top)
                {
                    _topCheckPoint = checkPoint.transform;
                }
            }
        }
    }

    private void ChangeDirection()
    {
        _direction *= -1;
    }

    private void ChangeGravityDirection()
    {
        GravityDirection *= -1;
        Physics.gravity = new Vector3(Physics.gravity.x, Physics.gravity.y * -1, Physics.gravity.z);
    }

    public bool IsTouchWall()
    {
        return _isLeftCheck || _isRightCheck;
    }

    public bool IsTouchGround()
    {
        return GravityDirection == 1 ? _isBottomCheck : _isTopCheck;
    }

    public bool IsTouchSide(CheckSide checkSide)
    {
        switch (checkSide)
        {
            case CheckSide.Top:
                {
                    return _isTopCheck;
                }
            case CheckSide.Bottom:
                {
                    return _isBottomCheck;
                }
            case CheckSide.Right:
                {
                    return _isRightCheck;
                }
            case CheckSide.Left:
                {
                    return _isLeftCheck;
                }
        }
        return false;
    }
}