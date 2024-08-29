using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        Debug.Log("Trigger");
        EventBus.OnChangeGravityDirection?.Invoke();
    }
}
