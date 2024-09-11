using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChangeGravityObject : MonoBehaviour
{
    private bool _canChangeGravity;

    private void Start()
    {
        _canChangeGravity = true;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.GetComponent<MainPlayerCollider>() && _canChangeGravity)
        {
            EventBus.OnChangeGravityDirection?.Invoke();
            _canChangeGravity = false;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.GetComponent<MainPlayerCollider>())
        {
            _canChangeGravity = true;
        }
    }
}
