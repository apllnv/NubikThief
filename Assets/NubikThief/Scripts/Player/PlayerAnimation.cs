using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField]
    private float lerpDuration;

    private Transform _playerModel;
    private Transform _playerModelParent;
    private Animator _animator;

    private void OnEnable()
    {
        EventBus.OnPlayerChangeDirection += RotateModelAfterJumpFromWall;
        EventBus.OnChangeGravityDirection += FlipPlayerGraphic;
        EventBus.OnAnimatorSetBool += AnimatorSetBool;
    }

    private void OnDisable()
    {
        EventBus.OnPlayerChangeDirection -= RotateModelAfterJumpFromWall;
        EventBus.OnChangeGravityDirection -= FlipPlayerGraphic;
        EventBus.OnAnimatorSetBool -= AnimatorSetBool;
    }

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _playerModel = _animator.transform;
        _playerModelParent = _playerModel.parent;
        Debug.Log(_playerModel.name);
        Debug.Log(_playerModelParent.name);
    }

    private void AnimatorSetBool(string name, bool value)
    {
        _animator.SetBool(name, value);
    }

    private void RotateModelAfterJumpFromWall()
    {
        float yRotate = 180f;
        StartCoroutine(RotateModelCoroutine(0, yRotate, 0));
    }

    private void FlipPlayerGraphic()
    {
        float yRotate = 180f;
        float zRotate = 180f;
        StartCoroutine(FlipPlayerGraphicCoroutine(0f, yRotate, zRotate));
    }

    private IEnumerator RotateModelCoroutine(float x, float y, float z)
    {
        float timeElapsed = 0;
        Quaternion startRotation = _playerModel.localRotation;
        Quaternion targetRotation = _playerModel.localRotation * Quaternion.Euler(x, y, z);

        while (timeElapsed <= lerpDuration)
        {
            _playerModel.localRotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        _playerModel.localRotation = targetRotation;
    }

    private IEnumerator FlipPlayerGraphicCoroutine(float x, float y, float z)
    {
        float timeElapsed = 0;
        Quaternion startRotation = _playerModelParent.localRotation;
        Quaternion targetRotation = _playerModelParent.localRotation * Quaternion.Euler(x, y, z);

        Vector3 startPos = _playerModelParent.localPosition;
        Vector3 targetPos = _playerModelParent.localPosition * -1;

        while (timeElapsed <= lerpDuration)
        {
            _playerModelParent.localRotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed / lerpDuration);
            _playerModelParent.localPosition = Vector3.Lerp(startPos, targetPos, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        _playerModelParent.localRotation = targetRotation;
        _playerModelParent.localPosition = targetPos;
    }
}
