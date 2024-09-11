using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitRotate : MonoBehaviour
{
    [SerializeField]
    private Transform _centerOfRotate;

    [SerializeField]
    private float _rotateAnglePerSeconds;

    private void Start()
    {
        _centerOfRotate.parent = null;
        transform.parent = _centerOfRotate;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        float angle = _rotateAnglePerSeconds * Time.fixedDeltaTime;
        Vector3 newRotation = new Vector3(0, 0, angle);
        _centerOfRotate.eulerAngles += newRotation;
    }
}